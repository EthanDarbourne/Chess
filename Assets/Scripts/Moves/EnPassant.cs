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

        public Square CaptureOn => _captureOn;

        protected override void DoExecuteMove( Board board )
        {
            // en passant moves the current pawn one square behind the adjacent pawn
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

        public override void ExecuteShallowMove( ShallowBoard board)
        {
            (int rank1, int file1) = (From.Rank.Num, From.File.Num);
            (int rank2, int file2) = (To.Rank.Num, To.File.Num);
            (int rankCapture, int fileCapture) = (_captureOn.Rank.Num, _captureOn.File.Num);
            board.CaptureSquare( rank1, file1, rankCapture, fileCapture );
            board.SwapSquares( rankCapture, fileCapture, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
