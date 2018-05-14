using Ragnar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bubak.Client
{
    public enum TorrentState
    {
        QueuedForChecking = 0,
        CheckingFiles = 1,
        DownloadingMetadata = 2,
        Downloading = 3,
        Finished = 4,
        Seeding = 5,
        Allocating = 6,
        CheckingResumeData = 7
    }

    public enum TorrentPosition { Up, Down, Top, Bottom }

    public struct Torrent
    {
        public string Name { get; }
        public string Comment { get; }
        public DateTime? CreationDate { get; }
        public IReadOnlyList<File> Files { get; }
        public TimeSpan? ActiveTime { get; }
        public DateTime? AddedDate { get; }
        public DateTime? CompletedDate { get; }
        public int DownloadRate { get; }
        public string ErrorMessage { get; }
        public TimeSpan? FinishedTime { get; }

        public bool IsPaused { get; }
        public int Priority { get; }
        public float Progress { get; }
        public long TotalBytes => Files.Sum(f => f.TotalBytes);
        public long DownloadedBytes => Files.Sum(f => f.DownloadedBytes);
        public int QueuePosition { get; }
        public string SavePath { get; }
        public TimeSpan? SeedingTime { get; }
        public bool CanSeed { get; }
        public bool IsSequentialDownload { get; }
        public TorrentState State { get; }
        public IReadOnlyList<byte> ResumeData { get; }

        public string InfoHash { get; }

        public static Torrent FromTorrentInfo(TorrentInfo info) 
            => new Torrent(info.InfoHash, info.Name, info.Comment, info.CreationDate, FilesFromTorrentInfo(info).ToList().AsReadOnly());

        public static IEnumerable<File> FilesFromTorrentInfo(TorrentInfo info) => Enumerable
                .Range(0, info.NumFiles)
                .Select(i => File.FromFileEntry(info.FileAt(i)));

        public Torrent(string infoHash, string name, string comment, DateTime? creationDate, IReadOnlyList<File> files, 
            TimeSpan? activeTime = null, DateTime? addedDate = null, DateTime? completedDate = null, string savePath = null, 
            int downloadRate = -1, string errorMessage = null, TimeSpan? finishedTime = null,  bool isPaused = false, 
            int priority = -1, float progress = 0, int queuePosition = -1, TimeSpan? seedingTime = null, bool canSeed = false, 
            bool isSequentialDownload = false, TorrentState state = default(TorrentState), IReadOnlyList<byte> resumeData = null) : this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            InfoHash = infoHash ?? throw new ArgumentNullException(nameof(infoHash));
            Comment = comment;
            CreationDate = creationDate;
            Files = files ?? throw new ArgumentNullException(nameof(files));
            ActiveTime = activeTime;
            AddedDate = addedDate;
            CompletedDate = completedDate;
            DownloadRate = downloadRate;
            ErrorMessage = errorMessage;
            FinishedTime = finishedTime;
            IsPaused = isPaused;
            Priority = priority;
            Progress = progress;
            QueuePosition = queuePosition;
            SavePath = savePath;
            SeedingTime = seedingTime;
            CanSeed = canSeed;
            IsSequentialDownload = isSequentialDownload;
            State = state;
            ResumeData = resumeData;
            
        }

        public Torrent SetResumeData(IEnumerable<byte> resumeData) => resumeData == null
            ? throw new ArgumentNullException(nameof(resumeData))
            : new Torrent(InfoHash, Name, Comment, CreationDate, Files, ActiveTime, AddedDate, CompletedDate, 
                SavePath, DownloadRate, ErrorMessage, FinishedTime, IsPaused, Priority, Progress, QueuePosition, 
                SeedingTime, CanSeed, IsSequentialDownload, State, resumeData.ToList().AsReadOnly());

        public Torrent SetFileName(string name, int index) => SetFiles(Files.Select((f, i) => i == index ? f.SetName(name) : f));
        public Torrent SetFileCompleted(int index) => SetFiles(Files.Select((f, i) => i == index ? f.SetCompleted() : f));

        public Torrent SetFiles(IEnumerable<File> files) => files == null
            ? throw new ArgumentNullException(nameof(files))
            : new Torrent(InfoHash, Name, Comment, CreationDate, files.ToList().AsReadOnly(), ActiveTime, AddedDate, CompletedDate,
                SavePath, DownloadRate, ErrorMessage, FinishedTime, IsPaused, Priority, Progress, QueuePosition,
                SeedingTime, CanSeed, IsSequentialDownload, State, ResumeData);

        public Torrent Update(TorrentStatus status, IEnumerable<File> files) => status == null
            ? throw new ArgumentNullException(nameof(status))
            : new Torrent(InfoHash, Name, Comment, CreationDate, files.ToList().AsReadOnly(), status.ActiveTime, status.AddedTime,
                status.CompletedTime, status.SavePath, status.DownloadRate, status.Error, status.FinishedTime, status.Paused,
                status.Priority, status.Progress, status.QueuePosition, status.SeedingTime,
                status.SeedMode, status.SequentialDownload, (TorrentState)status.State, ResumeData);

        public Torrent Update(TorrentStatus status) => Update(status, Files);

        public Torrent Update(TorrentStatus status, IEnumerable<int> filePriorities, IEnumerable<long> fileProgresses) => Update(status, UpdateFiles(filePriorities, fileProgresses));

        public IEnumerable<File> UpdateFiles(IEnumerable<int> filePriorities, IEnumerable<long> fileProgresses) => Files
                .Zip(filePriorities, (f, priority) => new { f, priority })
                .Zip(fileProgresses, (a, progress) => new { a.f, a.priority, progress })
                .Select(a => a.f.SetDownloadedBytes(a.progress).SetPriority(a.priority));
    }
}
