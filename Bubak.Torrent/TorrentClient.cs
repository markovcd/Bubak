using System;
using System.Collections.Generic;
using System.Linq;

using Bubak.Shared.Misc;
using Ragnar;


namespace Bubak.Client
{
    public class TorrentClient : IDisposable
    {
        private ILogger _logger;
        private Session _session;
        private TorrentClientSettings _settings;

        public IReadOnlyList<Torrent> Torrents { get; private set; }

        public TorrentClientSettings Settings
        {
            get => _settings ?? (_settings = new TorrentClientSettings());
            set => _settings = value;
        }

        public TorrentClient(ILogger logger = null)
        {
            _session = new Session();
            _logger = logger ?? new DebugLogger();
            Torrents = new List<Torrent>();
        }

        public Torrent AddTorrent(string url)
        {
            var parameters = CreateParams(url);
            var handle = _session.AddTorrent(parameters);

            if (handle == null)
            {
                OnFailedAddTorrent(url);
                return null;
            }

            var torrent = new Torrent(handle);

            if (Torrents == null) Torrents = new List<Torrent>();

            Torrents = Torrents
                .Concat(new[] { torrent })
                .ToList()
                .AsReadOnly();

            OnAddTorrent(torrent);
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

        protected virtual void OnFailedAddTorrent(string url)
        {
            _logger.Log($"Failed adding torrent: {url}");
        }

        protected virtual void OnAddTorrent(Torrent torrent)
        {
            _logger.Log($"Added torrent: {torrent.Name}");
        }

        public void Dispose()
        {
            foreach (var torrent in Torrents) torrent.Dispose();
            _session?.Dispose();
            _session = null;
            _logger = null;
        }
    }
}
