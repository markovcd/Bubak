using System;
using System.Net;

namespace Bubak.Client.Wrappers
{
    [Flags]
    public enum PeerFlags : uint
    {
        Interesting = 1,
        Choked = 2,
        RemoteInterested = 4,
        RemoteChoked = 8,
        SupportsExtensions = 16,
        LocalConnection = 32,
        Handshake = 64,
        Connecting = 128,
        Queued = 256,
        OnParole = 512,
        Seed = 1024,
        OptimisticUnchoke = 2048,
        Snubbed = 4096,
        UploadOnly = 8192,
        EndgameMode = 16384,
        Holepunched = 32768,
        I2pSocket = 65536,
        UtpSocket = 131072,
        SslSocket = 262144,
        Rc4Encrypted = 1048576,
        PlainTextEncrypted = 2097152
    }

    public class PeerInfo
    {
        public int TimedOutRequests { get; }
        public int InetAs { get; }
        public string InetAsName { get; }
        public string CountryCode { get; }
        public int NumHashfails { get; }
        public int UsedReceiveBuffer { get; }
        public int ReceiveBufferSize { get; }
        public int UsedSendBuffer { get; }
        public int SendBufferSize { get; }
        public ValueType RequestTimeout { get; }
        public int QueueBytes { get; }
        public ValueType DownloadQueueTime { get; }
        public ValueType LastActive { get; }
        public ValueType LastRequest { get; }
        public int DownloadLimit { get; }
        public int UploadLimit { get; }
        public long TotalUpload { get; }
        public long TotalDownload { get; }
        public int PayloadDownSpeed { get; }
        public int PayloadUpSpeed { get; }
        public int DownSpeed { get; }
        public int UpSpeed { get; }
        public int DownloadQueueLength { get; }
        public IPEndPoint EndPoint { get; }
        public PeerFlags Flags { get; }
        public int RequestsInBuffer { get; }
        public IPEndPoint LocalEndPoint { get; }
        public int EstimatedReciprocationRate { get; }
        public int ProgressPpm { get; }
        public float Progress { get; }
        public int UploadRatePeak { get; }
        public int DownloadRatePeak { get; }
        public int NumPieces { get; }
        public int RoundTripTime { get; }
        public int ReceiveQuota { get; }
        public int BusyRequests { get; }
        public int SendQuota { get; }
        public int RemoteDownloadRate { get; }
        public string Client { get; }
        public int DownloadingTotal { get; }
        public int DownloadingProgress { get; }
        public int DownloadingBlockIndex { get; }
        public int DownloadingPieceIndex { get; }
        public int FailCount { get; }
        public int UploadQueueLength { get; }
        public int TargetDownloadQueueLength { get; }
        public int PendingDiskBytes { get; }

        public PeerInfo()
        {
        }

        public PeerInfo(Ragnar.PeerInfo peerInfo)
        {
            try
            {
                TimedOutRequests = peerInfo.TimedOutRequests;
                InetAs = peerInfo.InetAs;
                InetAsName = peerInfo.InetAsName;
                CountryCode = peerInfo.CountryCode;
                NumHashfails = peerInfo.NumHashfails;
                UsedReceiveBuffer = peerInfo.UsedReceiveBuffer;
                ReceiveBufferSize = peerInfo.ReceiveBufferSize;
                UsedSendBuffer = peerInfo.UsedSendBuffer;
                SendBufferSize = peerInfo.SendBufferSize;
                RequestTimeout = peerInfo.RequestTimeout;
                QueueBytes = peerInfo.QueueBytes;
                DownloadQueueTime = peerInfo.DownloadQueueTime;
                LastActive = peerInfo.LastActive;
                LastRequest = peerInfo.LastRequest;
                DownloadLimit = peerInfo.DownloadLimit;
                UploadLimit = peerInfo.UploadLimit;
                TotalUpload = peerInfo.TotalUpload;
                TotalDownload = peerInfo.TotalDownload;
                PayloadDownSpeed = peerInfo.PayloadDownSpeed;
                PayloadUpSpeed = peerInfo.PayloadUpSpeed;
                DownSpeed = peerInfo.DownSpeed;
                UpSpeed = peerInfo.UpSpeed;
                DownloadQueueLength = peerInfo.DownloadQueueLength;
                EndPoint = peerInfo.EndPoint;
                Flags = (PeerFlags)peerInfo.Flags;
                RequestsInBuffer = peerInfo.RequestsInBuffer;
                LocalEndPoint = peerInfo.LocalEndPoint;
                EstimatedReciprocationRate = peerInfo.EstimatedReciprocationRate;
                ProgressPpm = peerInfo.ProgressPpm;
                Progress = peerInfo.Progress;
                UploadRatePeak = peerInfo.UploadRatePeak;
                DownloadRatePeak = peerInfo.DownloadRatePeak;
                NumPieces = peerInfo.NumPieces;
                RoundTripTime = peerInfo.RoundTripTime;
                ReceiveQuota = peerInfo.ReceiveQuota;
                BusyRequests = peerInfo.BusyRequests;
                SendQuota = peerInfo.SendQuota;
                RemoteDownloadRate = peerInfo.RemoteDownloadRate;
                Client = peerInfo.Client;
                DownloadingTotal = peerInfo.DownloadingTotal;
                DownloadingProgress = peerInfo.DownloadingProgress;
                DownloadingBlockIndex = peerInfo.DownloadingBlockIndex;
                DownloadingPieceIndex = peerInfo.DownloadingPieceIndex;
                FailCount = peerInfo.FailCount;
                UploadQueueLength = peerInfo.UploadQueueLength;
                TargetDownloadQueueLength = peerInfo.TargetDownloadQueueLength;
                PendingDiskBytes = peerInfo.PendingDiskBytes;
            }
            finally
            {
                peerInfo.Dispose();
            }
        }
    }
}