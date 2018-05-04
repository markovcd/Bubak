using Ragnar;

namespace Bubak.Client
{
    public class File : IFile
    {
        public long TotalBytes { get; }
        public string Path { get; }
        public long DownloadedBytes { get; set; }      
        public int Priority { get; set; }
        public bool IsFinished { get; set; }

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
