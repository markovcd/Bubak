using Bubak.Client;
using System;

namespace Bubak.ViewModel
{
    public class TorrentWrapperFactory : ITorrentWrapperFactory
    {
        public ITorrentWrapper Create(Torrent torrent)
        {
            return new TorrentWrapper(torrent ?? throw new ArgumentNullException(nameof(torrent)));
        }
    }
}
