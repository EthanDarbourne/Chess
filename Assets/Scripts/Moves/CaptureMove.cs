using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Moves
{
    public class CaptureMove : Move
    {
        private Piece _capturedPiece;

        public CaptureMove( Square from, Square to, bool isCheck = false, bool isCheckmate = false )
            : base(from, to, isCheck, isCheckmate)
        {

        }

        protected override void DoExecuteMove( Board board )
        {
            // todo: move to side of board
            Piece movingPiece = From.RemovePiece();
            To.CapturePiece( movingPiece );
        }

        protected override void DoUndoMove( Board board )
        {
            Piece movingPiece = To.RemovePiece();
            From.MovePieceTo( movingPiece );

            _capturedPiece.Uncapture();
            To.MovePieceTo( _capturedPiece );
        }
    }
}
