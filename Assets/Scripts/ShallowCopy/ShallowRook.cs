using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System.Collections.Generic;

namespace Assets.Scripts.ShallowCopy
{
    public class ShallowRook : ShallowPiece
    {
        public ShallowRook( int rank, int file, ChessColor color ) : base( rank, file, color )
        {
        }

        public ShallowRook( ChessColor color ) : base( color )
        {
        }

        public override PieceType Type => PieceType.Rook;

        protected override List<ShallowMove> GetPotentialMoves( ShallowBoard board )
        {
            // check forward moves
            List<ShallowMove> res = board.GetRookMoves( _rank, _file, Color );

            return res;
        }
    }
}
