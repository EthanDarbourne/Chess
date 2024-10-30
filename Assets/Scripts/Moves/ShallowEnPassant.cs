using Assets.Scripts.Parts;

namespace Assets.Scripts.Moves
{
    public class ShallowEnPassant : ShallowMove
    {
        private ShallowBoard.Square _captureOn;
     
        public ShallowEnPassant( ShallowBoard.Square from, ShallowBoard.Square to, ShallowBoard.Square captureOn, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
            _captureOn = captureOn;
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int rank1, int file1) = (From.Rank, From.File);
            (int rank2, int file2) = (To.Rank, To.File);
            (int rankCapture, int fileCapture) = (_captureOn.Rank, _captureOn.File);
            board.CaptureSquare( rank1, file1, rankCapture, fileCapture );
            board.SwapSquares( rankCapture, fileCapture, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
