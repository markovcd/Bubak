using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bubak.Client
{
    public interface ITorrentClient
    {
        TorrentClientSettings Settings { get; set; }
        IReadOnlyList<Torrent> Torrents { get; }

        event TorrentHandler TorrentAdded;
        event TorrentHandler TorrentChecked;
        event TorrentFileHandler TorrentFileCompleted;
        event TorrentFileNameHandler TorrentFileRenamed;
        event TorrentHandler TorrentFinished;
        event TorrentHandler TorrentMetadataReceived;
        event TorrentHandler TorrentPaused;
        event TorrentHandler TorrentRemoved;
        event TorrentHandler TorrentResumed;
        event TorrentResumeDataHandler TorrentResumeDataSaved;
        event TorrentStateChangeHandler TorrentStateChanged;
        event TorrentHandler TorrentStatsReceived;
        event TorrentHandler TorrentUpdated;

        Task<Torrent> AddTorrentAsync(string url);
        void Pause();
        Task<Torrent> RemoveTorrentAsync(Torrent torrent, bool removeData = false);
        void Resume();
    }
}