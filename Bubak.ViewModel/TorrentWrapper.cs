using Bubak.Client;
using Caliburn.Micro;

namespace Bubak.ViewModel
{
    public class TorrentWrapper : PropertyChangedBase, ITorrentWrapper
    {
        private Torrent _torrent;
        public Torrent Torrent
        {
            get => _torrent;
            set => Set(ref _torrent, value);
        }

        public TorrentWrapper(Torrent torrent)
        {
            Torrent = torrent;
            //Torrent.Updated += (t) => NotifyOfPropertyChange(() => Torrent);
        }
    }
}
