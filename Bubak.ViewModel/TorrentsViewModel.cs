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
        private readonly ITorrentWrapperFactory _torrentWrapperFactory;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private IDictionary<string, ITorrentWrapper> _torrents;
        public IObservableCollection<ITorrentWrapper> Torrents { get; }

        public TorrentClientSettings Settings => _client.Settings;

        public TorrentsViewModel(ITorrentClient client, IEventAggregator eventAggregator, ILogger logger, ITorrentWrapperFactory torrentWrapperFactory)
        {
            _logger = logger;
            _client = client;
            _eventAggregator = eventAggregator;
            _torrentWrapperFactory = torrentWrapperFactory;
            Torrents = new BindableCollection<ITorrentWrapper>();
            _torrents = new Dictionary<string, ITorrentWrapper>();
            
            _client.TorrentUpdated += Client_TorrentUpdated;
        }

        private void Client_TorrentUpdated(ITorrentClient sender, Torrent torrent)
        {
            _torrents[torrent.InfoHash].Torrent = torrent;
        }

        public async Task<ITorrentWrapper> AddTorrentAsync(string url)
        {
            var torrent = await _client.AddTorrentAsync(url);

            var wrapper = _torrentWrapperFactory.Create(torrent);
            _torrents.Add(torrent.InfoHash, wrapper);
            Torrents.Add(wrapper);

            return wrapper;
        }

        public async Task<bool> RemoveTorrentAsync(ITorrentWrapper torrentVm, bool removeData)
        {
            await _client.RemoveTorrentAsync(torrentVm.Torrent, removeData).TimeoutAfter(_timeout).ConfigureAwait(false);

            return Torrents.Remove(torrentVm) && _torrents.Remove(torrentVm.Torrent.InfoHash);
        }

        public void Dispose()
        {
            (_client as IDisposable)?.Dispose();
            _client = null;
        }
    }
}
