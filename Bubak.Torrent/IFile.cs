namespace Bubak.Client
{
    public interface IFile
    {
        long DownloadedBytes { get; set; }
        bool IsFinished { get; set; }
        string Path { get; }
        int Priority { get; set; }
        long TotalBytes { get; }
    }
}