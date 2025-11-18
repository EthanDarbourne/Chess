using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.ShallowCopy;
using System;
using System.Collections.Generic;
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

            // rank and file is the square the pawn is moving to
            void CreateMoves( int rank, int file, bool isCapture = false )
            {
                if ( rank == Utilities.GetPromotionRankForPawn( Color ) )
                {
                    // create 4 moves, one for each promotion type
                    Func<PieceType, Move> moveCreator = type =>
                    {
                        Square square = board.GetSquare( rank, file );
                        return isCapture ? MoveCreator.CreatePromotionCaptureMove( board, from, square, type ) :
                                MoveCreator.CreatePromotionBasicMove( board, from, square, type );
                    };

                    foreach(PieceType pieceType in Utilities.PromotionPieceTypes )
                    {
                        res.Add( moveCreator( pieceType ) );
                    }
                }
                else
                {
                    // create normal move
                    Square square = board.GetSquare( rank, file );
                    res.Add( isCapture ? MoveCreator.CreateCaptureMove( board, from, square ) :
                                MoveCreator.CreateBasicMove( board, from, square ) );
                }
            }

            // check forward moves
            if ( board.IsFree( rank + oneStep, file ) )
            {
                CreateMoves( rank + oneStep, file );

                if ( !_hasMoved && board.IsFree( rank + twoStep, file ) )
                {
                    CreateMoves( rank + twoStep, file );
                }
            }

            // check diagonal capture
            if ( board.CanCapture( rank + oneStep, leftFile, Color ) )
            {
                // make capture move
                CreateMoves( rank + oneStep, leftFile, true );
            }

            if ( board.CanCapture( rank + oneStep, rightFile, Color ) )
            {
                CreateMoves( rank + oneStep, rightFile, true );
            }

            if ( rank == Utilities.GetEnPassantRankForPawn( Color ) )
            {
                // check en-passant
                Square? leftSquare = board.GetSquareOrDefault( rank, leftFile );
                if ( leftSquare?.Piece is Pawn lPawn && lPawn.Color != Color && board.LastMove?.To == leftSquare && board.LastMove?.GetRankLength() == 2 )
                {
                    // can en-passant left side
                    Square moveSquare = board.GetSquare( rank + oneStep, leftFile );
                    res.Add( MoveCreator.CreateEnPassantMove( board, from, moveSquare, leftSquare ) );
                }

                Square? rightSquare = board.GetSquareOrDefault( rank, rightFile );
                if ( rightSquare?.Piece is Pawn rPawn && rPawn.Color != Color && board.LastMove?.To == rightSquare && board.LastMove?.GetRankLength() == 2 )
                {
                    // can en-passant right side
                    Square moveSquare = board.GetSquare( rank + oneStep, rightFile );
                    res.Add( MoveCreator.CreateEnPassantMove( board, from, moveSquare, rightSquare ) );
                }
            }

            return res;
        }

        public override ShallowPiece CreateShallowPiece() =>
            new ShallowPawn( _location.Rank.Num, _location.File.Num, Color );

        public override void Move( int rankChange, int fileChange )
        {
            _hasMoved = true;
            base.Move( rankChange, fileChange );
        }

    }
}
