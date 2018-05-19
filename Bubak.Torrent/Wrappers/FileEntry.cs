namespace Bubak.Client.Wrappers
{
    public class FileEntry
    {
        public long FileBase { get; }

        public long Size { get; }

        public long Offset { get; }

        public string Path { get; }

        public FileEntry()
        {
        }

        public FileEntry(Ragnar.FileEntry fileEntry)
        {
            try
            {
                FileBase = fileEntry.FileBase;
                Size = fileEntry.Size;
                Offset = fileEntry.Offset;
                Path = fileEntry.Path;
            }
            finally
            {
                fileEntry.Dispose();
            }
        }
    }
}
