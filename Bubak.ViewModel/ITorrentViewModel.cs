using Bubak.Client;

namespace Bubak.ViewModel
{
    public interface ITorrentViewModel
    {
        ITorrent Torrent { get; }
        string Url { get; }
    }
}