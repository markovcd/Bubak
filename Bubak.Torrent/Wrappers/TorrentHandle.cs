using System;
using System.Collections.Generic;
using System.Linq;

namespace Bubak.Client.Wrappers
{
    public class TorrentHandle : ITorrentHandle, IDisposable
    {
        private Ragnar.TorrentHandle _handle;

        public int MaxUploads { get => _handle.MaxUploads; set => _handle.MaxUploads = value; }
        public int DownloadLimit { get => _handle.DownloadLimit; set => _handle.DownloadLimit = value; }
        public bool HasMetadata => _handle.HasMetadata;
        public int UploadLimit { get => _handle.UploadLimit; set => _handle.UploadLimit = value; }
        public string InfoHash => _handle.InfoHash.ToHex();
        public bool IsSeed => _handle.IsSeed;
        public int MaxConnections { get => _handle.MaxConnections; set => _handle.MaxConnections = value; }
        public bool IsPaused => _handle.IsPaused;
        public TorrentInfo TorrentFile => new TorrentInfo(_handle.TorrentFile); 
        public bool ResolveCountries { get => _handle.ResolveCountries; set => _handle.ResolveCountries = value; }
        public bool IsFinished => _handle.IsFinished;
        public bool SequentialDownload { get => _handle.SequentialDownload; set => _handle.SequentialDownload = value; }
        public bool SuperSeeding { get => _handle.SuperSeeding; set => _handle.SuperSeeding = value; }
        public int QueuePosition => _handle.QueuePosition;
        public bool AutoManaged { get => _handle.AutoManaged; set => _handle.AutoManaged = value; }

        public TorrentHandle()
        {         
        }

        public TorrentHandle(Ragnar.TorrentHandle torrentHandle)
        {
            _handle = torrentHandle ?? throw new ArgumentNullException(nameof(torrentHandle));
        }

        public void ApplyIPFilter(bool value) => _handle.ApplyIPFilter(value);

        public void ClearError() => _handle.ClearError();

        public void ClearPieceDeadlines() => _handle.ClearPieceDeadlines();

        public void FlushCache() => _handle.FlushCache();

        public void ForceDhtAnnounce() => _handle.ForceDhtAnnounce();

        public void ForceReannounce() => _handle.ForceReannounce();

        public void ForceReannounce(int seconds, int trackerIndex) => _handle.ForceReannounce(seconds, trackerIndex);

        public void ForceRecheck() => _handle.ForceRecheck();

        public IEnumerable<PartialPieceInfo> GetDownloadQueue() => _handle
            .GetDownloadQueue()
            .Select(p => new PartialPieceInfo(p))
            .ToList();

        public int[] GetFilePriorities() => _handle.GetFilePriorities();

        public int GetFilePriority(int fileIndex) => _handle.GetFilePriority(fileIndex);

        public long[] GetFileProgresses() => _handle.GetFileProgresses();

        public IEnumerable<PeerInfo> GetPeerInfo() => _handle
            .GetPeerInfo()
            .Select(p => new PeerInfo(p))
            .ToList();

        public IEnumerable<AnnounceEntry> GetTrackers() => _handle
            .GetTrackers()
            .Select(p => new AnnounceEntry(p))
            .ToList();

        public bool HavePiece(int pieceIndex) => _handle.HavePiece(pieceIndex);

        public void MoveStorage(string savePath, MoveFlags flags) => _handle.MoveStorage(savePath, (Ragnar.TorrentHandle.MoveFlags)flags);

        public bool NeedSaveResumeData() => _handle.NeedSaveResumeData();

        public void Pause() => _handle.Pause();

        public TorrentStatus QueryStatus() => new TorrentStatus(_handle.QueryStatus());

        public void QueuePositionBottom() => _handle.QueuePositionBottom();

        public void QueuePositionDown() => _handle.QueuePositionDown();

        public void QueuePositionTop() => _handle.QueuePositionTop();

        public void QueuePositionUp() => _handle.QueuePositionUp();

        public void ReadPiece(int pieceIndex) => _handle.ReadPiece(pieceIndex);

        public void RenameFile(int fileIndex, string fileName) => _handle.RenameFile(fileIndex, fileName);

        public void ResetPieceDeadline(int pieceIndex) => _handle.ResetPieceDeadline(pieceIndex);

        public void Resume() => _handle.Resume();

        public void SaveResumeData() => _handle.SaveResumeData();

        public void ScrapeTracker() => _handle.ScrapeTracker();

        public void SetFilePriorities(int[] filePriorities) => _handle.SetFilePriorities(filePriorities);

        public void SetFilePriority(int fileIndex, int priority) => _handle.SetFilePriority(fileIndex, priority);

        public void SetPieceDeadline(int pieceIndex, int deadline) => _handle.SetPieceDeadline(pieceIndex, deadline);

        public void SetPriority(int priority) => _handle.SetPriority(priority);

        public void SetShareMode(bool value) => _handle.SetShareMode(value);

        public void SetTrackerLogin(string userName, string password) => _handle.SetTrackerLogin(userName, password);

        public void SetUploadMode(bool value) => _handle.SetUploadMode(value);

        public void Dispose()
        {
            _handle.Dispose();
            _handle = null;
        }
    }
}
