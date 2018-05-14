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
        private readonly Func<Torrent, string, ITorrentWrapper> _torrentViewModelCreator;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public IObservableCollection<ITorrentWrapper> Torrents { get; }

        public TorrentsViewModel(ITorrentClient client, IEventAggregator eventAggregator, ILogger logger, Func<Torrent, string, ITorrentWrapper> torrentViewModelCreator)
        {
            _logger = logger;
            _client = client;
            _eventAggregator = eventAggregator;
            _torrentViewModelCreator = torrentViewModelCreator;
            Torrents = new BindableCollection<ITorrentWrapper>();
        }

        public async Task<ITorrentWrapper> AddTorrentAsync(string url)
        {
            var torrent = await _client.AddTorrentAsync(url).TimeoutAfter(_timeout);
            var torrentVm = _torrentViewModelCreator(torrent, url);
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
