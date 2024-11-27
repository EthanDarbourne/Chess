using Assets.Scripts.Enums;
using Assets.Scripts.Parts;

namespace Assets.Scripts.Moves
{
    public class ShallowPromotionCaptureMove : ShallowCaptureMove
    {
        private PieceType _promoteTo;
        public ShallowPromotionCaptureMove( ShallowBoard.Square from, ShallowBoard.Square to, PieceType promoteTo, bool isCheck = false, bool isCheckmate = false )
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
