using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public interface ITorrentsViewModel
    {
        IObservableCollection<ITorrentWrapper> Torrents { get; }
        Task<ITorrentWrapper> AddTorrentAsync(string url);
    }
}