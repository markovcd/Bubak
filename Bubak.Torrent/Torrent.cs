using Ragnar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public class Torrent : IDisposable
    {
        internal TorrentHandle _handle;
        private TimeSpan _timeout;

        public string Name { get; }
        public string Comment { get; }
        public DateTime? CreationDate { get; }
        public IReadOnlyList<File> Files { get; }
        public TimeSpan ActiveTime { get; private set; }
        public DateTime AddedDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public int DownloadRate { get; private set; }
        public string ErrorMessage { get; private set; }
        public TimeSpan FinishedTime { get; private set; }

        public bool IsPaused { get; private set; }
        public int Priority { get; private set; }
        public float Progress { get; private set; }
        public long TotalBytes => Files.Sum(f => f.TotalBytes);
        public long DownloadedBytes => Files.Sum(f => f.DownloadedBytes);
        public int QueuePosition { get; private set; }
        public string SavePath { get; private set; }
        public TimeSpan SeedingTime { get; private set; }
        public bool CanSeed { get; private set; }
        public bool IsSequentialDownload { get; private set; }
        public TorrentState State { get; private set; }

        public Torrent(TorrentHandle handle)
        {
            _handle = handle;
            _timeout = TimeSpan.FromSeconds(1);

            TorrentInfo info = null;

            var stopwatch = Stopwatch.StartNew();
            while (info == null && stopwatch.Elapsed <= _timeout)
            {
                info = _handle.TorrentFile;
            }

            stopwatch.Stop();

            Name = info.Name;
            Comment = info.Comment;
            CreationDate = info.CreationDate;

            Files = Enumerable
                .Range(0, info.NumFiles)
                .Select(info.FileAt)
                .Select(f => new File(f))
                .ToList()
                .AsReadOnly();
        }

        public void Update()
        {
            UpdateStatus(_handle.QueryStatus());
            UpdateFileProperties(_handle.GetFilePriorities(), (f, p) => f.Priority = p);
            UpdateFileProperties(_handle.GetFileProgresses(), (f, p) => f.DownloadedBytes = p);
        }

        public void Pause()
        {
            _handle.Pause();
        }

        public void Resume()
        {
            _handle.Resume();
        }

        private void UpdateStatus(TorrentStatus status)
        {
            ActiveTime = status.ActiveTime;
            AddedDate = status.AddedTime;
            CompletedDate = status.CompletedTime;
            DownloadRate = status.DownloadRate;
            ErrorMessage = status.Error;
            FinishedTime = status.FinishedTime;
            IsPaused = status.Paused;
            Priority = status.Priority;
            Progress = status.Progress;
            QueuePosition = status.QueuePosition;
            SavePath = status.SavePath;
            SeedingTime = status.SeedingTime;
            CanSeed = status.SeedMode;
            IsSequentialDownload = status.SequentialDownload;
            State = (TorrentState)status.State;
            
            status.Dispose();
        }

        private void UpdateFileProperties<T>(IEnumerable<T> vector, Action<File, T> updateAction)
        {
            var i = 0;
            foreach (var item in vector) updateAction(Files[i++], item);
        }

        public void Dispose()
        {
            _handle?.Dispose();
            _handle = null;
        }
    }
}
