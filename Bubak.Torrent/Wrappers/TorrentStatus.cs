using System;

namespace Bubak.Client.Wrappers
{
    public class TorrentStatus
    {
        public int BlockSize { get; }
        public int DistributedFullCopies { get; }
        public int NumPieces { get; }
        public int ConnectCandidates { get; }
        public int ListPeers { get; }
        public int ListSeeds { get; }
        public int NumIncomplete { get; }
        public int NumComplete { get; }
        public int NumPeers { get; }
        public int NumSeeds { get; }
        public int UploadPayloadRate { get; }
        public int DownloadPayloadRate { get; }
        public int UploadRate { get; }
        public int DownloadRate { get; }
        public int QueuePosition { get; }
        public int DistributedFraction { get; }
        public float Progress { get; }
        public DateTime? CompletedTime { get; }
        public DateTime AddedTime { get; }
        public long AllTimeDownload { get; }
        public long AllTimeUpload { get; }
        public long TotalWanted { get; }
        public long TotalWantedDone { get; }
        public long TotalDone { get; }
        public long TotalReduntantBytes { get; }
        public long TotalFailedBytes { get; }
        public long TotalPayloadUpload { get; }
        public long TotalPayloadDownload { get; }
        public long TotalUpload { get; }
        public long TotalDownload { get; }
        public string Name { get; }
        public DateTime? LastSeenComplete { get; }
        public float DistributedCopies { get; }
        public string Error { get; }
        public int NumUploads { get; }
        public string InfoHash { get; }
        public bool MovingStorage { get; }
        public bool SeedMode { get; }
        public bool HasIncoming { get; }
        public bool HasMetadata { get; }
        public bool IsFinished { get; }
        public bool IsSeeding { get; }
        public bool SequentialDownload { get; }
        public bool AutoManaged { get; }
        public bool Paused { get; }
        public bool SuperSeeding { get; }
        public bool ShareMode { get; }
        public bool UploadMode { get; }
        public bool IPFilterApplies { get; }
        public string SavePath { get; }
        public bool NeedSaveResume { get; }
        public int Priority { get; }
        public int NumConnections { get; }
        public int UploadsLimit { get; }
        public int ConnectionsLimit { get; }
        public int UpBandwidthQueue { get; }
        public int DownBandwidthQueue { get; }
        public TorrentState State { get; }
        public TimeSpan TimeSinceUpload { get; }
        public TimeSpan ActiveTime { get; }
        public TimeSpan FinishedTime { get; }
        public TimeSpan SeedingTime { get; }
        public int SeedRank { get; }
        public TimeSpan? LastScrape { get; }
        public int SparseRegions { get; }
        public TimeSpan TimeSinceDownload { get; }

        public TorrentStatus()
        {
        }

        public TorrentStatus(Ragnar.TorrentStatus torrentStatus)
        {
            try
            {
                BlockSize = torrentStatus.BlockSize;
                DistributedFullCopies = torrentStatus.DistributedFullCopies;
                NumPieces = torrentStatus.NumPieces;
                ConnectCandidates = torrentStatus.ConnectCandidates;
                ListPeers = torrentStatus.ListPeers;
                ListSeeds = torrentStatus.ListSeeds;
                NumIncomplete = torrentStatus.NumIncomplete;
                NumComplete = torrentStatus.NumComplete;
                NumPeers = torrentStatus.NumPeers;
                NumSeeds = torrentStatus.NumSeeds;
                UploadPayloadRate = torrentStatus.UploadPayloadRate;
                DownloadPayloadRate = torrentStatus.DownloadPayloadRate;
                UploadRate = torrentStatus.UploadRate;
                DownloadRate = torrentStatus.DownloadRate;
                QueuePosition = torrentStatus.QueuePosition;
                DistributedFraction = torrentStatus.DistributedFraction;
                Progress = torrentStatus.Progress;
                CompletedTime = torrentStatus.CompletedTime;
                AddedTime = torrentStatus.AddedTime;
                AllTimeDownload = torrentStatus.AllTimeDownload;
                AllTimeUpload = torrentStatus.AllTimeUpload;
                TotalWanted = torrentStatus.TotalWanted;
                TotalWantedDone = torrentStatus.TotalWantedDone;
                TotalDone = torrentStatus.TotalDone;
                TotalReduntantBytes = torrentStatus.TotalReduntantBytes;
                TotalFailedBytes = torrentStatus.TotalFailedBytes;
                TotalPayloadUpload = torrentStatus.TotalPayloadUpload;
                TotalPayloadDownload = torrentStatus.TotalPayloadDownload;
                TotalUpload = torrentStatus.TotalUpload;
                TotalDownload = torrentStatus.TotalDownload;
                Name = torrentStatus.Name;
                LastSeenComplete = torrentStatus.LastSeenComplete;
                DistributedCopies = torrentStatus.DistributedCopies;
                Error = torrentStatus.Error;
                NumUploads = torrentStatus.NumUploads;
                InfoHash = torrentStatus.InfoHash.ToHex();
                MovingStorage = torrentStatus.MovingStorage;
                SeedMode = torrentStatus.SeedMode;
                HasIncoming = torrentStatus.HasIncoming;
                HasMetadata = torrentStatus.HasMetadata;
                IsFinished = torrentStatus.IsFinished;
                IsSeeding = torrentStatus.IsSeeding;
                SequentialDownload = torrentStatus.SequentialDownload;
                AutoManaged = torrentStatus.AutoManaged;
                Paused = torrentStatus.Paused;
                SuperSeeding = torrentStatus.SuperSeeding;
                ShareMode = torrentStatus.ShareMode;
                UploadMode = torrentStatus.UploadMode;
                IPFilterApplies = torrentStatus.IPFilterApplies;
                SavePath = torrentStatus.SavePath;
                NeedSaveResume = torrentStatus.NeedSaveResume;
                Priority = torrentStatus.Priority;
                NumConnections = torrentStatus.NumConnections;
                UploadsLimit = torrentStatus.UploadsLimit;
                ConnectionsLimit = torrentStatus.ConnectionsLimit;
                UpBandwidthQueue = torrentStatus.UpBandwidthQueue;
                DownBandwidthQueue = torrentStatus.DownBandwidthQueue;
                State = (TorrentState)torrentStatus.State;
                TimeSinceUpload = torrentStatus.TimeSinceUpload;
                ActiveTime = torrentStatus.ActiveTime;
                FinishedTime = torrentStatus.FinishedTime;
                SeedingTime = torrentStatus.SeedingTime;
                SeedRank = torrentStatus.SeedRank;
                LastScrape = torrentStatus.LastScrape;
                SparseRegions = torrentStatus.SparseRegions;
                TimeSinceDownload = torrentStatus.TimeSinceDownload;
            }
            finally 
            {
                torrentStatus.Dispose();
            }
        }
    }
}
