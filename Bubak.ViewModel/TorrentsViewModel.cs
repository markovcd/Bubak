using Bubak.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Bubak.Shared.Misc;
using Bubak.Shared;

namespace Bubak.ViewModel
{
    public class TorrentsViewModel : PropertyChangedBase, ITorrentsViewModel, IDisposable
    {
        private ITorrentClient _client;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;
        private readonly Func<Torrent, ITorrentWrapper> _torrentWrapperCreator;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private IDictionary<string, ITorrentWrapper> _torrents;
        public IObservableCollection<ITorrentWrapper> Torrents { get; }

        public TorrentsViewModel(ITorrentClient client, IEventAggregator eventAggregator, ILogger logger, Func<Torrent, ITorrentWrapper> torrentWrapperCreator)
        {
            _logger = logger;
            _client = client;
            _eventAggregator = eventAggregator;
            _torrentWrapperCreator = torrentWrapperCreator;
            Torrents = new BindableCollection<ITorrentWrapper>();
            _torrents = new Dictionary<string, ITorrentWrapper>();
            
            _client.TorrentAdded += Client_TorrentAdded;
            _client.TorrentRemoved += Client_TorrentRemoved;
            _client.TorrentUpdated += Client_TorrentUpdated;
        }

        private void Client_TorrentUpdated(ITorrentClient sender, Torrent torrent)
        {
            _torrents[torrent.InfoHash].Torrent = torrent;
        }

        private void Client_TorrentRemoved(ITorrentClient sender, Torrent torrent)
        {
            Torrents.Remove(_torrents[torrent.InfoHash]);
            _torrents.Remove(torrent.InfoHash);
        }

        private void Client_TorrentAdded(ITorrentClient sender, Torrent torrent)
        {
            var wrapper = _torrentWrapperCreator(torrent);
            _torrents.Add(torrent.InfoHash, wrapper);
            Torrents.Add(wrapper);
        }

        public async Task<ITorrentWrapper> AddTorrentAsync(string url)
        {
            var torrent = await _client.AddTorrentAsync(url).TimeoutAfter(_timeout);
            var torrentVm = _torrentWrapperCreator(torrent);
            Torrents.Add(torrentVm);

            return torrentVm;
        }

        public async Task<bool> RemoveTorrentAsync(ITorrentWrapper torrentVm, bool removeData)
        {
            await _client.RemoveTorrentAsync(torrentVm.Torrent, removeData).TimeoutAfter(_timeout);

            return Torrents.Remove(torrentVm);
        }

        public void Dispose()
        {
            (_client as IDisposable)?.Dispose();
            _client = null;
        }
    }
}
