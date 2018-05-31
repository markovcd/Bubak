using Bubak.Shared.Misc;
using Ragnar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bubak.Client
{
    public partial class TorrentClient : ITorrentClient, IDisposable
    {
        private ILogger _logger;
        private ISession _session;

        private TorrentClientSettings _settings;
        public TorrentClientSettings Settings
        {
            get => _settings ?? (_settings = new TorrentClientSettings());
            set => _settings = value ?? throw new NullReferenceException(nameof(value));
        }

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

        public TorrentClient(ILogger logger = null)
            : this(logger ?? new DebugLogger(), new Session())
        {
        }

        public TorrentClient(ILogger logger, ISession session)
        {
            _logger = logger;
            _session = session;
            _session.SetAlertMask(SessionAlertCategory.All);
            _torrents = new Dictionary<string, (Torrent torrent, TorrentHandle handle)>();

            StartEventLoop();
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

            if (!TryGetTorrent(torrent.InfoHash, out TorrentHandle handle))
                throw new KeyNotFoundException();

            _session.RemoveTorrent(handle, removeData);

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            StopEventLoop();

            if (disposing)
            {
                (_session as IDisposable)?.Dispose();
                (_logger as IDisposable)?.Dispose();
                _session = null;
                _logger = null;
            }
        }

        ~TorrentClient()
        {
            Dispose(false);
        }
    }
}
