using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bubak.Shared.Misc;
using Ragnar;

namespace Bubak.Client
{
    public partial class TorrentClient : IDisposable, ITorrentClient
    {
        private readonly Func<TorrentHandle, ITorrent> _torrentCreator;
        private ILogger _logger;
        private ISession _session;
        private TorrentClientSettings _settings;
        private TimeSpan _timeout;
        private volatile bool _isLoopRunning;
        private readonly object _torrentsLock;
        private volatile ITorrent _torrentToRemove;

        private IReadOnlyList<ITorrent> _torrents;
        public IReadOnlyList<ITorrent> Torrents
        {
            get
            {
                lock (_torrentsLock)
                {
                    return _torrents.ToList().AsReadOnly();
                }
            }
        }

        public TorrentClientSettings Settings
        {
            get => _settings ?? (_settings = new TorrentClientSettings());
            set => _settings = value;
        }

        public TorrentClient(ILogger logger = null) 
            : this(logger ?? new DebugLogger(), new Session(), h => new Torrent(h, e => new File(e)))
        {
        }

        public TorrentClient(ILogger logger, ISession session, Func<TorrentHandle, ITorrent> torrentCreator)
        {
            _torrentCreator = torrentCreator;
            _torrentsLock = new object();
            _timeout = TimeSpan.FromMilliseconds(10000);
            _logger = logger;
            _session = session;
            _session.SetAlertMask(SessionAlertCategory.All);
            _torrents = new List<ITorrent>();

            StartEventLoop();
        }

        public void StartEventLoop()
        {
            if (_isLoopRunning) throw new InvalidOperationException("Can't start more than one message loop.");
            _isLoopRunning = true;

            Task.Run(EventLoop);
        }

        private async Task EventLoop()
        {
            try
            {
                while (_isLoopRunning)
                {
                    if (_session.Alerts.PeekWait(_timeout))
                    {
                        var alerts = _session.Alerts.PopAll();
                        if (alerts != null)
                        {
                            foreach (var alert in alerts)
                            {
                                RaiseEvent(alert);
                            }
                        }
                    }
                   
                    await Task.Delay(100).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            finally
            {
                _isLoopRunning = false;
                StartEventLoop();
            }
        }

        public void StopEventLoop()
        {
            _isLoopRunning = false;
        }

        protected ITorrent GetTorrentByHandle(TorrentHandle handle)
        {
            lock (_torrentsLock)
            {
                var hashCode = handle?.TorrentFile?.InfoHash?.GetHashCode();
                if (hashCode == null) return null;
                return _torrents.FirstOrDefault(t => t.GetHashCode() == hashCode);
            }
        }

        protected ITorrent EnsureTorrentExist(TorrentHandle handle)
        {
            lock (_torrentsLock)
            {
                var torrent = GetTorrentByHandle(handle);
                
                if (torrent == null)
                {
                    if (handle == null) return null;
                    torrent = _torrentCreator(handle);

                    _torrents = Torrents
                        .Concat(new[] { torrent })
                        .ToList()
                        .AsReadOnly();

                    TorrentAdded?.Invoke(this, torrent);
                }

                return torrent;
            }

        }

        private bool RemoveTorrentFromList(TorrentHandle handle)
        {
            lock (_torrentsLock)
            {
                var torrent = GetTorrentByHandle(handle);
                if (torrent == null) return false;
                return RemoveTorrentFromList(torrent);
            }
        }

        private bool RemoveTorrentFromList(ITorrent torrent)
        {
            lock (_torrentsLock)
            {
                var remove = Torrents.Contains(torrent);

                if (remove)
                {
                    _torrents = Torrents
                        .Where(t => !ReferenceEquals(t, torrent))
                        .ToList()
                        .AsReadOnly();
                }

                return remove;
            }
        }

        protected AddTorrentParams CreateParams(string url)
        {
            return new AddTorrentParams
            {
                Url = url,
                SavePath = Settings.SavePath
            };
        }

        public void AddTorrent(string url)
        {
            _session.AsyncAddTorrent(CreateParams(url));
        }

        public Task<ITorrent> AddTorrentAsync(string url)
        {
            var tcs = new TaskCompletionSource<ITorrent>();
            TorrentAdded += (c, t) => tcs.SetResult(t);
            AddTorrent(url);
            return tcs.Task;
        }

        public void RemoveTorrent(ITorrent torrent, bool removeData = false)
        {
            if (torrent == null) return;
            while (_torrentToRemove != null) { }           
            _torrentToRemove = torrent;
            torrent.Remove(_session, removeData);
        }

        public async Task RemoveTorrentAsync(ITorrent torrent, bool removeData = false)
        {
            if (torrent == null) return;
            await Task.Run(() => 
            {
                while (_torrentToRemove != null) { }
            }).ConfigureAwait(false);

            _torrentToRemove = torrent;
            torrent.Remove(_session, removeData);
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

            lock (_torrentsLock)
            {
                if (_torrents != null)
                {
                    foreach (var torrent in _torrents)
                    {
                        (torrent as IDisposable)?.Dispose();
                    }
                }

                _torrents = null;
            }

            (_session as IDisposable)?.Dispose();
            _session = null;
            _logger = null;
        }
    }
}
