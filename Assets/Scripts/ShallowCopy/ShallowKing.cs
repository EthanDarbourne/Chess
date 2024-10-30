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
    public class ShallowKing : ShallowPiece
    {
        public ShallowKing( int rank, int file, ChessColor color ) : base( rank, file, color )
        {
        }

        public ShallowKing( ChessColor color ) : base( color )
        {
        }

        public override PieceType Type => PieceType.King;

        protected override List<ShallowMove> GetPotentialMoves( ShallowBoard board )
        {
            throw new NotImplementedException();
        }
    }
}
