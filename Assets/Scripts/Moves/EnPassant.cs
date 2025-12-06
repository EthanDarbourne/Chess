using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;

namespace Assets.Scripts.Moves
{
    public class EnPassant : Move
    {
        private Square _captureOn;

        public EnPassant( Square from, Square to, Square captureOn, string boardHash, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, boardHash, isCheck, isCheckmate )
        {
            _captureOn = captureOn;
        }

        public Square CaptureOn => _captureOn;

        protected override void DoExecuteMove( Board board )
        {
            // en passant moves the current pawn one square behind the adjacent pawn
            PieceGraveyard pieceGraveyard = board.GetPieceGraveyard( _captureOn.Piece.Color );
            _captureOn.CapturePiece(pieceGraveyard);

            Piece movingPiece = From.RemovePiece();
            To.MovePieceTo( movingPiece );
        }

        protected override void DoUndoMove( Board board )
        {
            Piece movingPiece = To.RemovePiece();
            From.MovePieceTo( movingPiece );

            PieceGraveyard pieceGraveyard = board.GetPieceGraveyard(Utilities.FlipTurn(movingPiece.Color));
            Piece revivedPiece = pieceGraveyard.RevivePiece(PieceType.Pawn);
            board.ClaimPiece(revivedPiece);

            _captureOn.MovePieceTo(revivedPiece);
        }

        public override void ExecuteShallowMove( ShallowBoard board)
        {
            (int rank1, int file1) = (From.Rank.Num, From.File.Num);
            (int rank2, int file2) = (To.Rank.Num, To.File.Num);
            (int rankCapture, int fileCapture) = (_captureOn.Rank.Num, _captureOn.File.Num);
            board.CaptureSquare( rank1, file1, rankCapture, fileCapture );
            board.SwapSquares( rankCapture, fileCapture, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
