using Bubak.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public class TorrentViewModel : BindableBase
    {
        public Torrent Torrent { get; }

        public TorrentViewModel(Torrent torrent)
        {
            Torrent = torrent;
            Torrent.Updated += (t) => OnPropertyChanged(nameof(Torrent));
        }
    }
}
