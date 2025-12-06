using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System.Collections.Generic;

namespace Assets.Scripts.ShallowCopy
{
    public class ShallowBishop : ShallowPiece
    {
        public ShallowBishop( int rank, int file, ChessColor color ) : base( rank, file, color )
        {
        }

        public ShallowBishop( ChessColor color ) : base( color )
        {
        }

        public override PieceType Type => PieceType.Bishop;

        protected override List<ShallowMove> GetPotentialMoves( ShallowBoard board )
        {
            // check forward moves
            List<ShallowMove> res = board.GetBishopMoves( _rank, _file, Color );

            return res;
        }
    }
}
