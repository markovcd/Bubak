using Bubak.Client;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public class TorrentViewModel : PropertyChangedBase, ITorrentViewModel
    {
        public ITorrent Torrent { get; }
        public string Url { get; }

        public TorrentViewModel(string url, ITorrent torrent)
        {
            Torrent = torrent;
            Url = url;
            Torrent.Updated += (t) => NotifyOfPropertyChange(() => Torrent);
        }
    }
}
