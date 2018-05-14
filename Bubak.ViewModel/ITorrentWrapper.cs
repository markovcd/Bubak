using Bubak.Client;

namespace Bubak.ViewModel
{
    public interface ITorrentWrapper
    {
        Torrent Torrent { get; }
        string Url { get; }
    }
}