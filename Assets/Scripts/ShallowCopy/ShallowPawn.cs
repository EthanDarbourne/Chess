using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System.Collections.Generic;
using static UnityEditor.FilePathAttribute;

namespace Assets.Scripts.ShallowCopy
{
    public class ShallowPawn : ShallowPiece
    {
        public ShallowPawn( int rank, int file, ChessColor color ) : base( rank, file, color )
        {
        }

        public ShallowPawn( ChessColor color ) : base( color )
        {
        }

        public override PieceType Type => PieceType.Pawn;

        protected override List<ShallowMove> GetPotentialMoves( ShallowBoard board )
        {
            // check all valid moves for a pawn
            int rank = _rank;
            int file = _file;

            List<ShallowMove> res = new();
            ShallowBoard.Square from = board.GetSquare( rank, file );

            int oneStep = Color == ChessColor.White ? 1 : -1;
            int twoStep = oneStep * 2;

            int leftFile = file - 1;
            int rightFile = file + 1;

            // check forward moves
            if ( board.IsFree( rank + oneStep, file ) )
            {
                res.Add( MoveCreator.CreateShallowBasicMove( board, from, board.GetSquare( rank + oneStep, file ) ) );

                if ( rank != Utilities.GetDefaultRankForPawn(Color) && board.IsFree( rank + twoStep, file ) )
                {
                    res.Add( MoveCreator.CreateShallowBasicMove( board, from, board.GetSquare( rank + twoStep, file ) ) );
                }
            }

            // check diagonal capture
            if ( board.CanCapture( rank + oneStep, leftFile, Color ) )
            { // make can capture
                res.Add( MoveCreator.CreateShallowCaptureMove( board, from, board.GetSquare( rank + oneStep, leftFile ) ) );
            }

            if ( board.CanCapture( rank + oneStep, rightFile, Color ) )
            {
                res.Add( MoveCreator.CreateShallowCaptureMove( board, from, board.GetSquare( rank + oneStep, rightFile ) ) );
            }

            // check en-passant
            ShallowBoard.Square? leftSquare = board.GetSquareOrDefault( rank, leftFile );
            if ( leftSquare?.Type == PieceType.Pawn && leftSquare?.Piece.Color != Color && board.LastMove?.To == leftSquare )
            {
                // can en-passant left side
                ShallowBoard.Square moveSquare = board.GetSquare( rank + oneStep, leftFile );
                res.Add( MoveCreator.CreateShallowEnPassant( board, from, moveSquare, leftSquare.Value ) );
            }

            ShallowBoard.Square? rightSquare = board.GetSquareOrDefault( rank, rightFile );
            if ( rightSquare?.Type == PieceType.Pawn && rightSquare?.Piece.Color != Color && board.LastMove?.To == rightSquare )
            {
                // can en-passant right side
                ShallowBoard.Square moveSquare = board.GetSquare( rank + oneStep, rightFile );
                res.Add( MoveCreator.CreateShallowEnPassant( board, from, moveSquare, rightSquare.Value ) );
            }
            return res;
        }

        
    }
}
