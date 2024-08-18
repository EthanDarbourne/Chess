using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Moves
{
    public class BasicMove : Move
    {
        public BasicMove(Square from, Square to, bool isCheck = false, bool isCheckmate = false )
            : base(from, to, isCheck, isCheckmate)
        {

        }

        protected override void DoExecuteMove( Board board )
        {
            Piece movingPiece = From.RemovePiece();
            To.MovePieceTo( movingPiece );
        }

        protected override void DoUndoMove( Board board )
        {
            Piece movingPiece = To.RemovePiece();
            From.MovePieceTo( movingPiece );
        }
    }
}
