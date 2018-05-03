using Ragnar;

namespace Bubak.Client
{
    public class File
    {
        public long TotalBytes { get; }
        public string Path { get; }
        public long DownloadedBytes { get; internal set; }      
        public int Priority { get; internal set; }
        public bool IsFinished { get; internal set; }

        internal File(string path, long totalBytes)
        {
            Path = path;
            TotalBytes = totalBytes;
        }

        internal File(FileEntry fileEntry)
            : this(fileEntry.Path, fileEntry.Size)
        {
        }
    }
}
