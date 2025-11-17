using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;

namespace Assets.Scripts.Moves
{
    public class CaptureMove : Move
    {
        private Piece _capturedPiece;

        public CaptureMove( Square from, Square to, bool isCheck = false, bool isCheckmate = false )
            : base(from, to, isCheck, isCheckmate)
        {

        }

        protected override void DoExecuteMove( Board board )
        {
            // todo: move to side of board
            Piece movingPiece = From.RemovePiece();
            To.CapturePiece( movingPiece );
        }

        protected override void DoUndoMove( Board board )
        {
            Piece movingPiece = To.RemovePiece();
            From.MovePieceTo( movingPiece );

            _capturedPiece.Uncapture();
            To.MovePieceTo( _capturedPiece );
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int rank1, int file1) = (From.Rank.Num, From.File.Num);
            (int rank2, int file2) = (To.Rank.Num, To.File.Num);
            board.CaptureSquare( rank1, file1, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
