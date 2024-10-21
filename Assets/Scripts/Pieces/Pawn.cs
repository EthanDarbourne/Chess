using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Pawn : Piece
    {
        private bool _hasMoved = false;

        public Pawn( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Pawn( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {

        }

        public override PieceType Type => PieceType.Pawn;

        protected override List<Move> GetPotentialMoves( Board board )
        {

            List<Move> res = new();
            Square from = board.GetSquare( Location );

            // check all valid moves for a pawn
            int file = _location.File.Num;
            int rank = _location.Rank.Num;

            int oneStep = Color == ChessColor.White ? 1 : -1;
            int twoStep = oneStep * 2;

            int leftFile = file - 1;
            int rightFile = file + 1;

            // check forward moves
            if ( board.CanMoveTo( rank + oneStep, file, Color ) )
            {
                res.Add( MoveCreator.CreateBasicMove( board, from, board.GetSquare( rank + oneStep, file ) ) );

                if ( !_hasMoved && board.CanMoveTo( rank + twoStep, file, Color ) )
                {
                    res.Add( MoveCreator.CreateBasicMove( board, from, board.GetSquare( rank + twoStep, file ) ) );
                }
            }

            // check diagonal capture
            if ( board.CanCapture( rank + oneStep, leftFile, Color ) )
            { // make can capture
                res.Add( MoveCreator.CreateCaptureMove( board, from, board.GetSquare( rank + oneStep, leftFile ) ) );
            }

            if ( board.CanCapture( rank + oneStep, rightFile, Color ) )
            {
                res.Add( MoveCreator.CreateCaptureMove( board, from, board.GetSquare( rank + oneStep, rightFile ) ) );
            }

            // check en-passant
            Square? leftSquare = board.GetSquareOrDefault( rank, leftFile );
            if ( leftSquare?.Piece is Pawn lPawn && lPawn.Color != Color && board.LastMove?.To == leftSquare )
            {
                // can en-passant left side
                Square moveSquare = board.GetSquare( rank + oneStep, leftFile );
                res.Add( MoveCreator.CreateEnPassantMove( board, from, moveSquare, leftSquare ) );
            }

            Square? rightSquare = board.GetSquareOrDefault( rank, rightFile );
            if ( rightSquare?.Piece is Pawn rPawn && rPawn.Color != Color && board.LastMove?.To == rightSquare )
            {
                // can en-passant right side
                Square moveSquare = board.GetSquare( rank + oneStep, rightFile );
                res.Add( MoveCreator.CreateEnPassantMove( board, from, moveSquare, rightSquare ) );
            }

            

            return res;
        }

        public override void Move( int rankChange, int fileChange )
        {
            _hasMoved = true;
            base.Move( rankChange, fileChange );
        }

    }
}
