using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.ShallowCopy
{
    public class ShallowKnight : ShallowPiece
    {
        public ShallowKnight( int rank, int file, ChessColor color ) : base( rank, file, color )
        {
        }

        public ShallowKnight( ChessColor color ) : base( color )
        {
        }

        public override PieceType Type => PieceType.Knight;

        protected override List<ShallowMove> GetPotentialMoves( ShallowBoard board )
        {
            List<ShallowMove> res = board.GetKnightMoves( _rank, _file, Color );

            return res;
        }
    }
}
