using System;

namespace Bubak.Client.Wrappers
{
    public class TorrentInfo : IDisposable
    {
        private Ragnar.TorrentInfo _torrentInfo;

        public int PieceLength => _torrentInfo.PieceLength;

        public string InfoHash => _torrentInfo.InfoHash;

        public int NumFiles => _torrentInfo.NumFiles;

        public string SslCert => _torrentInfo.SslCert;

        public bool IsValid => _torrentInfo.IsValid;

        public bool Private => _torrentInfo.Private;

        public DateTime? CreationDate => _torrentInfo.CreationDate;

        public string Name => _torrentInfo.Name;

        public string Comment => _torrentInfo.Comment;

        public string Creator => _torrentInfo.Creator;

        public int MetadataSize => _torrentInfo.MetadataSize;

        public bool IsMerkleTorrent => _torrentInfo.IsMerkleTorrent;

        public long TotalSize => _torrentInfo.TotalSize;

        public int NumPieces => _torrentInfo.NumFiles;

        public TorrentInfo()
        {
        }

        public TorrentInfo(Ragnar.TorrentInfo torrentInfo)
        {
            _torrentInfo = torrentInfo ?? throw new ArgumentNullException(nameof(torrentInfo));
        }

        public void AddTracker(string url) => _torrentInfo.AddTracker(url);

        public void AddTracker(string url, int tier) => _torrentInfo.AddTracker(url, tier);

        public FileEntry FileAt(int index) => new FileEntry(_torrentInfo.FileAt(index));

        public int PieceSize(int pieceIndex) => _torrentInfo.PieceSize(pieceIndex);

        public void RenameFile(int fileIndex, string fileName) => _torrentInfo.RenameFile(fileIndex, fileName);

        public void Dispose()
        {
            _torrentInfo?.Dispose();
            _torrentInfo = null;
        }
    }
}
