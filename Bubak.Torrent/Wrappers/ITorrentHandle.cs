using System.Collections.Generic;

namespace Bubak.Client.Wrappers
{
    public enum MoveFlags
    {
        AlwaysReplaceFiles = 0,
        FailIfExist = 1,
        DontReplace = 2
    }

    public interface ITorrentHandle
    {
        int MaxUploads { get; set; }
        int DownloadLimit { get; set; }
        bool HasMetadata { get; }
        int UploadLimit { get; set; }
        string InfoHash { get; }
        bool IsSeed { get; }
        int MaxConnections { get; set; }
        bool IsPaused { get; }
        TorrentInfo TorrentFile { get; }
        bool ResolveCountries { get; set; }
        bool IsFinished { get; }
        bool SequentialDownload { get; set; }
        bool SuperSeeding { get; set; }
        int QueuePosition { get; }
        bool AutoManaged { get; set; }

        void ApplyIPFilter(bool value);
        void ClearError();
        void ClearPieceDeadlines();
        void FlushCache();
        void ForceDhtAnnounce();
        void ForceReannounce();
        void ForceReannounce(int seconds, int trackerIndex);
        void ForceRecheck();
        IEnumerable<PartialPieceInfo> GetDownloadQueue();
        int[] GetFilePriorities();
        int GetFilePriority(int fileIndex);
        long[] GetFileProgresses();
        IEnumerable<PeerInfo> GetPeerInfo();
        IEnumerable<AnnounceEntry> GetTrackers();
        bool HavePiece(int pieceIndex);
        void MoveStorage(string savePath, MoveFlags flags);
        bool NeedSaveResumeData();
        void Pause();
        TorrentStatus QueryStatus();
        void QueuePositionBottom();
        void QueuePositionDown();
        void QueuePositionTop();
        void QueuePositionUp();
        void ReadPiece(int pieceIndex);
        void RenameFile(int fileIndex, string fileName);
        void ResetPieceDeadline(int pieceIndex);
        void Resume();
        void SaveResumeData();
        void ScrapeTracker();
        void SetFilePriorities(int[] filePriorities);
        void SetFilePriority(int fileIndex, int priority);
        void SetPieceDeadline(int pieceIndex, int deadline);
        void SetPriority(int priority);
        void SetShareMode(bool value);
        void SetTrackerLogin(string userName, string password);
        void SetUploadMode(bool value);
    }
}
