using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;

namespace Assets.Scripts.Moves
{
    public class Castling : Move
    {
        private Square _rookSquare;

        // from is king square, to is rook square (todo: from is king, to is clickable square)
        public Castling( Square from, Square to, Square rookSquare, string boardHash, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, boardHash, isCheck, isCheckmate )
        {
            _rookSquare = rookSquare;
        }

        public Square RookSquare => _rookSquare;

        public bool IsKingsideCastling => _rookSquare.File.Num > From.File.Num;

        protected override void DoExecuteMove( Board board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();

            Piece king = From.RemovePiece();
            Piece rook = _rookSquare.RemovePiece();

            Square rookNewSquare = board.GetSquare( To.Rank.Num, rookNewFile );
            Square kingNewSquare = board.GetSquare( To.Rank.Num, kingNewFile );

            rookNewSquare.MovePieceTo( rook );
            kingNewSquare.MovePieceTo( king );
        }

        protected override void DoUndoMove( Board board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();
            Square rookNewSquare = board.GetSquare( To.Rank.Num, rookNewFile );
            Square kingNewSquare = board.GetSquare( To.Rank.Num, kingNewFile );
            Piece king = kingNewSquare.RemovePiece();
            Piece rook = rookNewSquare.RemovePiece();
            From.MovePieceTo( king );
            _rookSquare.MovePieceTo( rook );
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();

            int rank = To.Rank.Num;
            board.SwapSquares( rank, From.File.Num, rank, kingNewFile );
            board.SwapSquares( rank, _rookSquare.File.Num, rank, rookNewFile );
            board.OnMoveExecuted();
        }

        private (int KingFile, int RookFile) GetNewKingAndRookFiles()
        {
            int kingFile = From.File.Num;
            int rookFile = _rookSquare.File.Num;
            if (rookFile < kingFile) // queenside
            {
                return (Constants.CASTLING_QUEEN_SIDE_KING_END, Constants.CASTLING_QUEEN_SIDE_ROOK_END);
            }
            else // kingside
            {
                return (Constants.CASTLING_KING_SIDE_KING_END, Constants.CASTLING_KING_SIDE_ROOK_END);
            }
        }
    }
}
