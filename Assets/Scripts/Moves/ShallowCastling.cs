using Assets.Scripts.Parts;
using System;

namespace Assets.Scripts.Moves
{
    public class ShallowCastling : ShallowMove
    {
        private ShallowBoard.Square _rookSquare;
        public ShallowCastling( ShallowBoard.Square from, ShallowBoard.Square to, ShallowBoard.Square rookSquare, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
            _rookSquare = rookSquare;
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();

            int rank = To.Rank;
            board.SwapSquares( rank, From.File, rank, kingNewFile );
            board.SwapSquares( rank, _rookSquare.File, rank, rookNewFile );
            board.OnMoveExecuted();
        }

        private (int KingFile, int RookFile) GetNewKingAndRookFiles()
        {
            int kingFile = From.File;
            int rookFile = _rookSquare.File;
            int dir = rookFile < kingFile ? -1 : 1;
            int kingNewFile = kingFile + dir * 2;
            int rookNewFile = kingNewFile - dir;
            return (kingNewFile, rookNewFile);
        }
    }
}
