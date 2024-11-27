using Assets.Scripts.Enums;
using Assets.Scripts.Parts;

namespace Assets.Scripts.Moves
{
    public class PromotionBasicMove : BasicMove
    {
        private PieceType _promoteTo;

        public PromotionBasicMove( Square from, Square to, PieceType promoteTo, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
            _promoteTo = promoteTo;
        }

        public PieceType PromoteTo => _promoteTo;

        protected override void DoExecuteMove( Board board )
        {
            base.DoExecuteMove( board );
            board.Promote( To, _promoteTo );
        }

        protected override void DoUndoMove( Board board )
        {
            board.Unpromote( To );
            base.DoUndoMove( board );
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            base.ExecuteShallowMove( board );
            (int rank2, int file2) = (To.Rank.Num, To.File.Num);
            board.Promote( rank2, file2, _promoteTo );
        }
    }
}
