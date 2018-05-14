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

        private ILogger _logger;
        private ISession _session;
        private TorrentClientSettings _settings;
        private TimeSpan _timeout;
        private volatile bool _isLoopRunning;
        private readonly object _torrentsLock;
        private string _torrentToRemove;


        private IDictionary<string, (Torrent torrent, TorrentHandle handle)> _torrents;
        public IReadOnlyList<Torrent> Torrents
        {
            get
            {
                lock (_torrentsLock)
                {
                    return _torrents.Select(kv => kv.Value.torrent).OrderBy(t => t.QueuePosition).ToList().AsReadOnly();
                }
            }
        } 

        public TorrentClientSettings Settings
        {
            get => _settings ?? (_settings = new TorrentClientSettings());
            set => _settings = value ?? throw new NullReferenceException(nameof(value));
        }

        public TorrentClient(ILogger logger = null) 
            : this(logger ?? new DebugLogger(), new Session())
        {
        }

        public TorrentClient(ILogger logger, ISession session)
        {
            _torrentsLock = new object();
            _timeout = TimeSpan.FromMilliseconds(10000);
            _logger = logger;
            _session = session;
            _session.SetAlertMask(SessionAlertCategory.All);
            _torrents = new Dictionary<string, (Torrent torrent, TorrentHandle handle)>();


            StartEventLoop();
        }

        protected void StartEventLoop()
        {
            if (_isLoopRunning) throw new InvalidOperationException("Can't start more than one message loop.");
            _isLoopRunning = true;
            _logger.Log("Starting event loop.");

            Task.Run(EventLoop);
        }

        private async Task EventLoop()
        {
            try
            {
                while (_isLoopRunning)
                {
                    _session.PostTorrentUpdates();
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
                   
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            finally
            {
                _isLoopRunning = false;
                _logger.Log("Event loop stopped.");
                StartEventLoop();
            }
        }

        protected void StopEventLoop()
        {
            _isLoopRunning = false;
        }

        protected bool TryGetTorrent(string infoHash, out Torrent torrent, out TorrentHandle handle)
        {
            lock (_torrentsLock)
            {
                if (infoHash == null) throw new NullReferenceException(nameof(infoHash));
                
                var result = _torrents.TryGetValue(infoHash, out var t);

                torrent = t.torrent;
                handle = t.handle;
                return result;
            }
        }

        protected bool TryGetTorrent(string infoHash, out Torrent torrent)
            => TryGetTorrent(infoHash, out torrent, out var handle);

        protected bool TryGetTorrent(string infoHash, out TorrentHandle handle)
            => TryGetTorrent(infoHash, out var torrent, out handle);

        protected Torrent AddTorrent(TorrentHandle handle)
        {
            lock (_torrentsLock)
            {
                var torrent = Torrent.FromTorrentInfo(handle.TorrentFile);
                _torrents.Add(handle.TorrentFile.InfoHash, (torrent, handle));
                TorrentAdded?.Invoke(this, torrent);
                return torrent;
            }
        }

        protected bool RemoveTorrent(string infoHash)
        {
            lock (_torrentsLock)
            {
                if (!TryGetTorrent(infoHash, out Torrent torrent)) return false;
                var result = _torrents.Remove(infoHash);
                TorrentRemoved?.Invoke(this, torrent);
                return true;
            }
        }

        protected Torrent EnsureTorrentExist(TorrentHandle handle)
        {
            return TryGetTorrent(handle.TorrentFile.InfoHash, out Torrent torrent)
                ? torrent 
                : AddTorrent(handle);
        }

        protected bool SetTorrent(Torrent torrent)
        {
            lock (_torrentsLock)
            {
                if (!_torrents.ContainsKey(torrent.InfoHash)) return false;
                _torrents[torrent.InfoHash] = ((torrent, _torrents[torrent.InfoHash].handle));
                TorrentUpdated?.Invoke(this, torrent);
                return true;
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

        public Task<Torrent> AddTorrentAsync(string url)
        {
            var tcs = new TaskCompletionSource<Torrent>();
            TorrentAdded += (c, t) => tcs.SetResult(t);
            _session.AsyncAddTorrent(CreateParams(url));
            return tcs.Task;
        }

        public async Task<Torrent> RemoveTorrentAsync(Torrent torrent, bool removeData = false)
        {
            var tcs = new TaskCompletionSource<Torrent>();
            TorrentRemoved += (c, t) => tcs.SetResult(t);

            await Task.Run(() => 
            {
                while (_torrentToRemove != null) { }
            }).ConfigureAwait(false);

            _torrentToRemove = torrent.InfoHash;
            _session.RemoveTorrent(_torrents[torrent.InfoHash].handle, removeData);

            return await tcs.Task;
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

            (_session as IDisposable)?.Dispose();
            (_logger as IDisposable)?.Dispose();
            _session = null;
            _logger = null;
        }
    }
}
