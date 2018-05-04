using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bubak.Client
{
    public interface ITorrentClient
    {
        TorrentClientSettings Settings { get; set; }
        IReadOnlyList<ITorrent> Torrents { get; }

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

        void AddTorrent(string url);
        Task<ITorrent> AddTorrentAsync(string url);
        void Pause();
        void RemoveTorrent(ITorrent torrent, bool removeData = false);
        Task RemoveTorrentAsync(ITorrent torrent, bool removeData = false);
        void Resume();
        void StartEventLoop();
        void StopEventLoop();
    }
}