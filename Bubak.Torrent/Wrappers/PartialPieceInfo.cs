namespace Bubak.Client.Wrappers
{
    public class PartialPieceInfo
    {
        public int Requested { get; }
        public int Writing { get; }
        public int Finished { get; }
        public int BlocksInPiece { get; }
        public int PieceIndex { get; }

        public PartialPieceInfo()
        {
        }

        public PartialPieceInfo(Ragnar.PartialPieceInfo partialPieceInfo)
        {
            try
            {
                Requested = partialPieceInfo.Requested;
                Writing = partialPieceInfo.Writing;
                Finished = partialPieceInfo.Finished;
                BlocksInPiece = partialPieceInfo.BlocksInPiece;
                PieceIndex = partialPieceInfo.PieceIndex;
            }
            finally
            {
                partialPieceInfo.Dispose();
            }
        }
    }
}
