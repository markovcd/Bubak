using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ragnar;

namespace Bubak.Client
{
    public partial class TorrentClient
    {          
        private readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(10000);
        private readonly object _torrentsLock = new object();

        private volatile bool _isLoopRunning;       
        private string _torrentToRemove;

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

                    _session.PostTorrentUpdates();

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

        protected Torrent UpdateTorrent(Torrent torrent)
        {
            if (!TryGetTorrent(torrent.InfoHash, out TorrentHandle handle)) throw new KeyNotFoundException();

            var status = handle.QueryStatus();
            var filePriorities = handle.GetFilePriorities();
            var fileProgresses = handle.GetFileProgresses();

            var newTorrent = torrent.Update(status, filePriorities, fileProgresses);
            if (!SetTorrent(newTorrent)) throw new InvalidOperationException();

            return newTorrent;
        }

        protected IEnumerable<Torrent> UpdateTorrents()
        {
            foreach (var torrent in _torrents.Values.Select(t => t.torrent))
            {
                yield return UpdateTorrent(torrent);
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

        
    }
}
