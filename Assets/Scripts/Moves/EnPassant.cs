using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Moves
{
    public class EnPassant : Move
    {
        private Square _captureOn;

        private Piece _capturedPiece;

        public EnPassant( Square from, Square to, Square captureOn, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
            _captureOn = captureOn;
        }

        protected override void DoExecuteMove( Board board )
        {
            // en passant moves the current pawn one square behind the adjacent pawn
            Debug.Log( "En passant" );
            _capturedPiece = _captureOn.CapturePiece();

            Piece movingPiece = From.RemovePiece();
            To.MovePieceTo( movingPiece );
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
