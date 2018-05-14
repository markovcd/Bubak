using Ragnar;
using System;

namespace Bubak.Client
{
    public struct File
    {
        public long TotalBytes { get; }
        public string Name { get; }
        public long DownloadedBytes { get; }
        public int Priority { get; }
        public bool IsFinished { get; }

        public File(string name, long totalBytes, long downloadedBytes = 0, int priority = -1, bool isFinished = false) : this()
        {
            TotalBytes = totalBytes;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DownloadedBytes = downloadedBytes;
            Priority = priority;
            IsFinished = isFinished;
        }

        public static File FromFileEntry(FileEntry entry) => new File(entry.Path, entry.Size);

        public File SetDownloadedBytes(long downloadedBytes) => new File(Name, TotalBytes, downloadedBytes, Priority, IsFinished);
        public File SetPriority(int priority) => new File(Name, TotalBytes, DownloadedBytes, priority, IsFinished);
        public File SetIsFinished(bool isFinished) => new File(Name, TotalBytes, DownloadedBytes, Priority, isFinished);
        public File SetName(string name) => new File(name, TotalBytes, DownloadedBytes, Priority, IsFinished);
        public File SetCompleted() => new File(Name, TotalBytes, DownloadedBytes, Priority, true);
    }
}
