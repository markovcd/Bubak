using Bubak.Client;
using Caliburn.Micro;

namespace Bubak.ViewModel
{
    public class TorrentWrapper : PropertyChangedBase, ITorrentWrapper
    {
        public Torrent Torrent { get; }
        public string Url { get; }

        public TorrentWrapper(string url, Torrent torrent)
        {
            Torrent = torrent;
            Url = url;
            //Torrent.Updated += (t) => NotifyOfPropertyChange(() => Torrent);
        }
    }
}
