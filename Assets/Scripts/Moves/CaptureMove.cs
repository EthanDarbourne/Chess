using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;

namespace Assets.Scripts.Moves
{
    public class CaptureMove : Move
    {
        private PieceType _capturedPieceType = PieceType.Empty;
        public CaptureMove( Square from, Square to, string boardHash, bool isCheck = false, bool isCheckmate = false )
            : base(from, to, boardHash, isCheck, isCheckmate)
        {

        }

        protected override void DoExecuteMove( Board board )
        {
            Piece movingPiece = From.RemovePiece();
            PieceGraveyard pieceGraveyard = board.GetPieceGraveyard( Utilities.FlipTurn( movingPiece.Color ));
            _capturedPieceType = To.Piece.Type;
            To.CapturePiece( movingPiece, board);
        }

        protected override void DoUndoMove( Board board )
        {
            Piece movingPiece = To.RemovePiece();
            From.MovePieceTo( movingPiece );

            PieceGraveyard pieceGraveyard = board.GetPieceGraveyard( Utilities.FlipTurn( movingPiece.Color ));
            Piece revivedPiece = pieceGraveyard.RevivePiece(_capturedPieceType);
            board.OnPieceRevived(revivedPiece);

            To.MovePieceTo(revivedPiece);
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
