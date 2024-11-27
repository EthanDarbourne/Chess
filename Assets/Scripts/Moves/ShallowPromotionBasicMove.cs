using Assets.Scripts.Enums;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Moves
{
    public class ShallowPromotionBasicMove : ShallowBasicMove
    {
        private PieceType _promoteTo;
        public ShallowPromotionBasicMove( ShallowBoard.Square from, ShallowBoard.Square to, PieceType promoteTo, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
            _promoteTo = promoteTo;
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            base.ExecuteShallowMove( board );
            (int rank2, int file2) = (To.Rank, To.File);
            board.Promote( rank2, file2, _promoteTo );
        }
    }
}
