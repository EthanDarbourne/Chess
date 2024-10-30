using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ShallowCopy
{
    public static class ShallowUtilities
    {
        public static int[] StraightMoves = { -1, 0, 1, 0, -1 };
        public static int[] DiagonalMoves = { -1, 1, 1, -1, -1 };
        public static int[] KnightMoves = { 2, 1, -2, -1, 2, -1, -2, 1, 2 };
        public static int[] BlackPawnMoves = { 1, -1, -1 };
        public static int[] WhitePawnMoves = { -1, 1, 1 };
        public static int[] AdjacentMoves = { 1, 0, -1, 0, 1, 0, -1, 0, 1 };
        public static PieceType[] StraightPieceTypes = { PieceType.Queen, PieceType.Rook };
        public static PieceType[] DiagonalPieceTypes = { PieceType.Queen, PieceType.Bishop };
        public static PieceType[] KnightPieceTypes = { PieceType.Knight };
        public static PieceType[] PawnTypes = { PieceType.Pawn };

        public static ChessColor FlipTurn( ChessColor color ) => color == ChessColor.White ? ChessColor.Black : ChessColor.White;

        // x and y are co-efficients for what direction the piece is trying to move into
        public static List<ShallowMove> GetMovesInDirection( this ShallowBoard board, int rank, int file, ChessColor color, int x, int y )
        {
            List<ShallowMove> res = new();
            ShallowBoard.Square from = board.GetSquare( rank, file );
            int i = 1;
            while ( board.CanMoveTo( rank + i * x, file + i * y, color ) )
            {
                res.Add( MoveCreator.CreateShallowMove( board, from, board.GetSquare( rank + i * x, file + i * y ) ) );
                if ( !board.IsFree( rank + i * x, file + i * y ) ) break;
                ++i;
            }
            return res;
        }

        // Get all valid rook moves from a square on the board
        public static List<ShallowMove> GetRookMoves( this ShallowBoard board, int rank, int file, ChessColor color )
        {
            List<ShallowMove> res = new();
            for ( int i = 0; i < StraightMoves.Length - 1; ++i )
            {
                res.AddRange( GetMovesInDirection( board, rank, file, color, StraightMoves[ i ], StraightMoves[ i + 1 ] ) );
            }
            return res;
        }

        // Get all valid bishop moves from a square on the board
        public static List<ShallowMove> GetBishopMoves( this ShallowBoard board, int rank, int file, ChessColor color )
        {
            List<ShallowMove> res = new();
            for ( int i = 0; i < DiagonalMoves.Length - 1; ++i )
            {
                res.AddRange( GetMovesInDirection( board, rank, file, color, DiagonalMoves[ i ], DiagonalMoves[ i + 1 ] ) );
            }
            return res;
        }

        public static List<ShallowMove> GetKnightMoves( this ShallowBoard board, int rank, int file, ChessColor color )
        {
            List<ShallowMove> res = new();
            ShallowBoard.Square from = board.GetSquare( rank, file );

            for ( int i = 0; i < KnightMoves.Length - 1; ++i )
            {
                (int rankTo, int fileTo) = (rank + KnightMoves[ i ], file + KnightMoves[ i + 1 ]);

                if ( board.CanMoveTo( rankTo, fileTo, color ) )
                {
                    res.Add( MoveCreator.CreateShallowMove( board, from, board.GetSquare( rankTo, fileTo ) ) );
                }
            }
            return res;
        }

        public static List<ShallowMove> GetKingMoves( this ShallowBoard board, int rank, int file, ChessColor color )
        {
            List<ShallowMove> res = new();
            ShallowBoard.Square from = board.GetSquare( rank, file );

            for ( int i = 0; i < AdjacentMoves.Length - 1; ++i )
            {
                (int rankTo, int fileTo) = (rank + AdjacentMoves[ i ], file + AdjacentMoves[ i + 1 ]);

                if ( board.CanMoveTo( rankTo, fileTo, color ) )
                {
                    res.Add( MoveCreator.CreateShallowMove( board, from, board.GetSquare( rankTo, fileTo ) ) );
                }
            }
            return res;
        }

        public static ShallowBoard.Square GetPieceInDirection( this ShallowBoard board, int rank, int file, int x, int y )
        {
            ShallowBoard.Square from = board.GetSquare( rank, file );
            int i = 1;
            while ( true )
            {
                (int nextRank, int nextFile) = (rank + i * x, file + i * y);
                if ( board.OutOfBounds( nextRank, nextFile ) ) return ShallowBoard.Square.Default;
                ShallowBoard.Square next = board.GetSquare( nextRank, nextFile );
                if ( next.IsCapturable( from.Piece.Color ) ) return next;
                if ( !next.IsFree ) break;
                ++i;
            }
            return ShallowBoard.Square.Default;
        }
    }
}
