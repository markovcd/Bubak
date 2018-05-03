using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bubak.Shared.Misc;
using Ragnar;

namespace Bubak.Client
{
    public partial class TorrentClient : IDisposable
    {
        private ILogger _logger;
        private ISession _session;
        private TorrentClientSettings _settings;
        private TimeSpan _timeout;
        private volatile bool _isLoopRunning;
               
        public IReadOnlyList<Torrent> Torrents { get; private set; }

        public TorrentClientSettings Settings
        {
            get => _settings ?? (_settings = new TorrentClientSettings());
            set => _settings = value;
        }

        public TorrentClient(ILogger logger = null) 
            : this(logger ?? new DebugLogger(), new Session())
        {
        }

        public TorrentClient(ILogger logger, ISession session)
        {
            _timeout = TimeSpan.FromMilliseconds(10000);
            _logger = logger;
            _session = session;
            _session.SetAlertMask(SessionAlertCategory.All);
            Torrents = new List<Torrent>();

            StartEventLoop();
        }

        public Torrent AddTorrent(string url)
        {
            var parameters = CreateParams(url);
            var handle = _session.AddTorrent(parameters);

            if (handle == null)
            {
                OnTorrentAddFailed(url);
                return null;
            }

            var torrent = new Torrent(handle);

            Torrents = Torrents
                .Concat(new[] { torrent })
                .ToList()
                .AsReadOnly();

            return torrent;
        }

        private AddTorrentParams CreateParams(string url)
        {
            return new AddTorrentParams
            {
                Url = url,
                SavePath = Settings.SavePath
            };
        }

        public void Update()
        {
            foreach (var torrent in Torrents) torrent.Update();
        }

        protected void StartEventLoop()
        {
            if (_isLoopRunning) throw new InvalidOperationException("Can't start more than one message loop.");
            _isLoopRunning = true;

            Task.Run(EventLoop);
        }

        private async Task EventLoop()
        {
            while (_isLoopRunning)
            {
                WaitForEvent();
                await Task.Delay(10).ConfigureAwait(false);
            }
        }

        protected void StopEventLoop()
        {
            _isLoopRunning = false;
        }

        protected bool WaitForEvent()
        {
            var result = _session.Alerts.PeekWait(_timeout);
            if (!result) return false;

            var alert = _session.Alerts.Pop();

            if (alert == null) return false;

            RaiseEvent(alert);

            return true;
        }

        protected Torrent GetTorrentByHandle(TorrentHandle handle)
        {
            return Torrents.FirstOrDefault(t => ReferenceEquals(handle, t._handle));
        }
   
        public void RemoveTorrent(Torrent torrent, bool removeData = false)
        {
            Torrents = Torrents
                .Where(t => !ReferenceEquals(t, torrent))
                .ToList()
                .AsReadOnly();

            _session.RemoveTorrent(torrent._handle, removeData);
            torrent.Dispose();        
        }

        public void Pause()
        {
            _session.Pause();
        }

        public void Resume()
        {
            _session.Resume();
        }

        public void Dispose()
        {
            StopEventLoop();
            foreach (var torrent in Torrents) torrent.Dispose();
            (_session as IDisposable)?.Dispose();
            _session = null;
            _logger = null;
        }
    }
}
