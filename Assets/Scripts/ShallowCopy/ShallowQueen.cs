using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ShallowCopy
{
    
    public class ShallowQueen : ShallowPiece
    {
        public ShallowQueen( int rank, int file, ChessColor color ) : base( rank, file, color )
        {
        }

        public ShallowQueen( ChessColor color ) : base( color )
        {
        }

        public override PieceType Type => PieceType.Queen;

        protected override List<ShallowMove> GetPotentialMoves( ShallowBoard board )
        {
            // check forward moves
            List<ShallowMove> res = board.GetRookMoves( _rank, _file, Color );
            res.AddRange( board.GetBishopMoves( _rank, _file, Color ) );

            return res;
        }
    }
}
