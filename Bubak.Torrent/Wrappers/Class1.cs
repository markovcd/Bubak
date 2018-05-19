
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubak.Client.Wrappers
{
    [Flags]
    public enum SessionAlertCategory : uint
    {
        Error = 1,
        Peer = 2,
        PortMapping = 4,
        Storage = 8,
        Tracker = 16,
        Debug = 32,
        Status = 64,
        Progress = 128,
        IPBlock = 256,
        Performance = 512,
        Dht = 1024,
        Stats = 2048,
        Rss = 4096,
        All = 2147483647
    }

    public class SessionSettings
    {
        public int MaxRejects { get; set; }
        public bool DontCountSlowTorrents { get; set; }
        public bool AutoManagePreferSeeds { get; set; }
        public int ActiveLimit { get; set; }
        public int ActiveLsdLimit { get; set; }
        public int ActiveTrackerLimit { get; set; }
        public int ActiveDhtLimit { get; set; }
        public int ActiveSeeds { get; set; }
        public int ActiveDownloads { get; set; }
        public bool CoalesceWrites { get; set; }
        public bool CoalesceReads { get; set; }
        public int DiskIOReadMode { get; set; }
        public int DiskIOWriteMode { get; set; }
        public int ExplicitCacheInterval { get; set; }
        public bool ExplicitReadCache { get; set; }
        public bool UseReadCache { get; set; }
        public int CacheExpiry { get; set; }
        public int CacheBufferChunkSize { get; set; }
        public int AutoManageInterval { get; set; }
        public float ShareRatioLimit { get; set; }
        public float SeedTimeRatioLimit { get; set; }
        public int SeedTimeLimit { get; set; }
        public int MaxSparseRegions { get; set; }
        public int SeedingPieceQuota { get; set; }
        public bool StrictSuperSeeding { get; set; }
        public bool PreferUdpTrackers { get; set; }
        public bool AnnounceToAllTiers { get; set; }
        public bool AnnounceToAllTrackers { get; set; }
        public bool RateLimitIPOverhead { get; set; }
        public int AutoManageStartup { get; set; }
        public int CacheSize { get; set; }
        public bool PrioritizePartialPieces { get; set; }
        public int MaxPausedPeerlistSize { get; set; }
        public int MaxPeerlistSize { get; set; }
        public int AutoScrapeMinInterval { get; set; }
        public int AutoScrapeInterval { get; set; }
        public bool CloseRedundantConnections { get; set; }
        public float PeerTurnoverCutoff { get; set; }
        public float PeerTurnover { get; set; }
        public int PeerTurnoverInterval { get; set; }
        public int MinAnnounceInterval { get; set; }
        public bool LockDiskCache { get; set; }
        public bool UseParoleMode { get; set; }
        public int ChokingAlgorithm { get; set; }
        public int MinReconnectTime { get; set; }
        public int MaxFailCount { get; set; }
        public bool AllowMultipleConnectionsPerIP { get; set; }
        public int FilePoolSize { get; set; }
        public int UrlSeedWaitRetry { get; set; }
        public int UrlSeedPipelineSize { get; set; }
        public int UrlSeedTimeout { get; set; }
        public int PeerTimeout { get; set; }
        public int WholePiecesThreshold { get; set; }
        public int MaxOutRequestQueue { get; set; }
        public int MaxAllowedInRequestQueue { get; set; }
        public int RequestQueueTime { get; set; }
        public int RequestTimeout { get; set; }
        public int PieceTimeout { get; set; }
        public int TrackerMaximumResponseLength { get; set; }
        public int StopTrackerTimeout { get; set; }
        public int TrackerReceiveTimeout { get; set; }
        public int PeerConnectTimeout { get; set; }
        public bool IgnoreLimitsOnLocalNetwork { get; set; }
        public int ConnectionSpeed { get; set; }
        public bool SendRedundantHave { get; set; }
        public int SendBufferWatermarkFactor { get; set; }
        public int SendBufferWatermark { get; set; }
        public int SendBufferLowWatermark { get; set; }
        public bool UpnpIgnoreNonRouters { get; set; }
        public bool FreeTorrentHashes { get; set; }
        public bool UseDhtAsFallback { get; set; }
        public int HandshakeTimeout { get; set; }
        public int MaxQueuedDiskBytesLowWatermark { get; set; }
        public int SeedChokingAlgorithm { get; set; }
        public int MaxQueuedDiskBytes { get; set; }
        public int AllowedFastSetSize { get; set; }
        public int InitialPickerThreshold { get; set; }
        public int NumWant { get; set; }
        public string AnnounceIP { get; set; }
        public int OptimisticUnchokeInterval { get; set; }
        public int UnchokeInterval { get; set; }
        public int InactivityTimeout { get; set; }
        public bool LazyBitfields { get; set; }
        public int SuggestMode { get; set; }
        public int TrackerCompletionTimeout { get; set; }
        public string UserAgent { get; set; }
        public int SendSocketBufferSize { get; set; }
        public int TorrentConnectBoost { get; set; }
        public bool AnnounceDoubleNat { get; set; }
        public int ListenQueueSize { get; set; }
        public bool RateLimitUtp { get; set; }
        public int MixedModeAlgorithm { get; set; }
        public int UtpLossMultiplier { get; set; }
        public bool UtpDynamicSocketBuffer { get; set; }
        public int UtpConnectTimeout { get; set; }
        public int UtpNumResends { get; set; }
        public int UtpFinResends { get; set; }
        public int UtpSynResends { get; set; }
        public int UtpMinTimeout { get; set; }
        public int UtpGainFactor { get; set; }
        public int UtpTargetDelay { get; set; }
        public int ConnectionsSlack { get; set; }
        public int ConnectionsLimit { get; set; }
        public int HalfOpenLimit { get; set; }
        public bool SeedingOutgoingConnections { get; set; }
        public int UnchokeSlotsLimit { get; set; }
        public bool NoConnectPrivilegedPorts { get; set; }
        public int MaxMetadataSize { get; set; }
        public int InactiveUpRate { get; set; }
        public int InactiveDownRate { get; set; }
        public bool UseDiskCachePool { get; set; }
        public string HandshakeClientVersion { get; set; }
        public bool ReportReduntantBytes { get; set; }
        public bool SupportMerkleTorrents { get; set; }
        public bool SupportShareMode { get; set; }
        public int MaxHttpReceiveBufferSize { get; set; }
        public bool BanWebSeeds { get; set; }
        public int TrackerBackoff { get; set; }
        public int SslListen { get; set; }
        public bool LockFiles { get; set; }
        public bool UseDiskReadAhead { get; set; }
        public int ReadJobEvery { get; set; }
        public bool ApplyIPFilterToTrackers { get; set; }
        public bool AlwaysSendUserAgent { get; set; }
        public bool SmoothConnects { get; set; }
        public int AlertQueueSize { get; set; }
        public int ReceiveSocketBufferSize { get; set; }
        public int DhtUploadRateLimit { get; set; }
        public int LocalUploadRateLimit { get; set; }
        public int DefaultCacheMinAge { get; set; }
        public bool GuidedReadCache { get; set; }
        public bool VolatileReadCache { get; set; }
        public int UdpTrackerTokenExpiry { get; set; }
        public int DhtAnnounceInterval { get; set; }
        public int LocalServiceAnnounceInterval { get; set; }
        public bool LowPrioDisk { get; set; }
        public bool DropSkippedRequests { get; set; }
        public int MaxSuggestPieces { get; set; }
        public bool AllowI2PMixed { get; set; }
        public bool AllowReorderedDiskOperations { get; set; }
        public bool DisableHashChecks { get; set; }
        public int OptimisticDiskRetry { get; set; }
        public int WriteCacheLineSize { get; set; }
        public int ReadCacheLineSize { get; set; }
        public int FileChecksDelayPerBlock { get; set; }
        public bool OptimizeHashingForSpeed { get; set; }
        public int NumOptimisticUnchokeSlots { get; set; }
        public int LocalDownloadRateLimit { get; set; }
        public int DefaultEstReciprocationRate { get; set; }
        public int DecreaseEstReciprocationRate { get; set; }
        public int DownloadRateLimit { get; set; }
        public int UploadRateLimit { get; set; }
        public int ShareModeTarget { get; set; }
        public bool ReportWebSeedDownloads { get; set; }
        public int TickInterval { get; set; }
        public bool ForceProxy { get; set; }
        public bool AnonymousMode { get; set; }
        public bool NoRecheckIncompleteResume { get; set; }
        public int IncreaseEstReciprocationRate { get; set; }
        public bool IgnoreResumeTimestamps { get; set; }
        public bool EnableIncomingTcp { get; set; }
        public bool EnableOutgoingTcp { get; set; }
        public bool EnableIncomingUtp { get; set; }
        public bool EnableOutgoingUtp { get; set; }
        public bool BroadcastLsd { get; set; }
        public bool StrictEndGameMode { get; set; }
        public bool ReportTrueDownloaded { get; set; }
        public bool IncomingStartsQueuedTorrents { get; set; }
        public int MaxPeerExchangePeers { get; set; }

        public SessionSettings()
        {
        }

        public SessionSettings(Ragnar.SessionSettings sessionSettings)
        {

        }
    }

    public class SessionStatus
    {
        public long TotalRedundantBytes { get; }
        public int TrackerDownloadRate { get; }
        public int TrackerUploadRate { get; }
        public long TotalDhtUpload { get; }
        public long TotalDhtDownload { get; }
        public int DhtDownloadRate { get; }
        public int DhtUploadRate { get; }
        public long TotalIPOverheadUpload { get; }
        public long TotalIPOverheadDownload { get; }
        public int IPOverheadDownloadRate { get; }
        public int IPOverheadUploadRate { get; }
        public long TotalPayloadUpload { get; }
        public long TotalPayloadDownload { get; }
        public int PayloadDownloadRate { get; }
        public int PayloadUploadRate { get; }
        public long TotalUpload { get; }
        public long TotalDownload { get; }
        public int DownloadRate { get; }
        public long TotalTrackerDownload { get; }
        public long TotalTrackerUpload { get; }
        public bool HasIncomingConnections { get; }
        public long TotalFailedBytes { get; }
        public int PeerlistSize { get; }
        public int DhtTotalAllocations { get; }
        public long DhtGlobalNodes { get; }
        public int DhtTorrents { get; }
        public int DhtNodeCache { get; }
        public int DhtNodes { get; }
        public int DiskReadQueue { get; }
        public int UploadRate { get; }
        public int DiskWriteQueue { get; }
        public int OptimisticUnchokeCounter { get; }
        public int DownBandwidthBytesQueue { get; }
        public int UpBandwidthBytesQueue { get; }
        public int DownBandwidthQueue { get; }
        public int UpBandwidthQueue { get; }
        public int AllowedUploadSlots { get; }
        public int NumUnchoked { get; }
        public int NumPeers { get; }
        public int UnchokeCounter { get; }
    }

    public abstract class Alert
    {
        public string Message { get; }
    }

    public interface IAlertFactory
    {
        bool PeekWait(TimeSpan timeout);
        Alert Pop();
        IEnumerable<Alert> PopAll();
    }

    public interface ISession
    {
        bool IsPaused { get; }
        bool IsListening { get; }
        int ListenPort { get; }
        int SslListenPort { get; }
        IAlertFactory Alerts { get; }
        bool IsDhtRunning { get; }

        TorrentHandle AddTorrent(Ragnar.AddTorrentParams parameters);
        void AsyncAddTorrent(Ragnar.AddTorrentParams parameters);
        TorrentHandle FindTorrent(string infoHash);
        IEnumerable<TorrentHandle> GetTorrents();
        void ListenOn(int lower, int upper);
        void LoadState(byte[] buffer);
        void Pause();
        void PostTorrentUpdates();
        SessionSettings QuerySettings();
        Ragnar.SessionStatus QueryStatus();
        void RemoveTorrent(TorrentHandle handle);
        void RemoveTorrent(TorrentHandle handle, bool removeData);
        void Resume();
        byte[] SaveState();
        void SetAlertMask(SessionAlertCategory mask);
        void SetKey(int key);
        void SetSettings(SessionSettings settings);
        void StartDht();
        void StartLsd();
        void StartNatPmp();
        void StartUpnp();
        void StopDht();
        void StopLsd();
        void StopNatPmp();
        void StopUpnp();
    }

    public class Session : ISession
    {

    }
}
