using Bubak.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public class TorrentsViewModel
    {
        public ObservableCollection<TorrentViewModel> Torrents { get; }

        public TorrentClient Client { get; }

        public TorrentsViewModel(TorrentClient client)
        {
            Client = client;
            Torrents = new ObservableCollection<TorrentViewModel>();

            Client.TorrentAdded += (c, t) => ClientTorrentAdded(t);
            Client.TorrentRemoved += (c, t) => ClientTorrentRemoved(t);
        }

        private void ClientTorrentRemoved(Torrent t)
        {
            throw new NotImplementedException();
        }

        private void ClientTorrentAdded(Torrent t)
        {
            throw new NotImplementedException();
        }
    }
}
