using Ragnar;
using System;
using System.Collections.Generic;

namespace Bubak.Client
{
    public interface ITorrent
    {
        TimeSpan ActiveTime { get; }
        DateTime AddedDate { get; }
        bool CanSeed { get; }
        string Comment { get; }
        DateTime? CompletedDate { get; }
        DateTime? CreationDate { get; }
        long DownloadedBytes { get; }
        int DownloadRate { get; }
        string ErrorMessage { get; }
        IReadOnlyList<IFile> Files { get; }
        TimeSpan FinishedTime { get; }
        string InfoHash { get; }
        bool IsPaused { get; }
        bool IsSequentialDownload { get; }
        string Name { get; }
        int Priority { get; }
        float Progress { get; }
        int QueuePosition { get; }
        byte[] ResumeData { get; set; }
        string SavePath { get; }
        TimeSpan SeedingTime { get; }
        TorrentState State { get; }
        long TotalBytes { get; }

        event Action<ITorrent> Updated;

        void Update();
        void Remove(ISession session, bool removeData = false);
    }
}