using Bubak.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public class TorrentViewModel : BindableBase, ITorrentViewModel
    {
        public ITorrent Torrent { get; }

        public TorrentViewModel(ITorrent torrent)
        {
            Torrent = torrent;
            Torrent.Updated += (t) => OnPropertyChanged(nameof(Torrent));
        }
    }
}
