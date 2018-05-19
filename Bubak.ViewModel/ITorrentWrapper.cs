using Bubak.Client;
using System.ComponentModel;

namespace Bubak.ViewModel
{
    public interface ITorrentWrapper : INotifyPropertyChanged
    {
        Torrent Torrent { get; }
        string Url { get; }
    }
}