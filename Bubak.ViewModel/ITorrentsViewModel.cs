using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bubak.ViewModel
{
    public interface ITorrentsViewModel
    {
        IObservableCollection<ITorrentViewModel> Torrents { get; }
        Task<ITorrentViewModel> AddTorrentAsync(string url);
    }
}