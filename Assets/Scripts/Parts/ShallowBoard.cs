using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;
using static Assets.Scripts.Parts.ShallowBoard;

namespace Assets.Scripts.Parts
{
    public class ShallowBoard
    {

        public record Square
        {
            public int Rank;
            public int File;
            public PieceType Type;
            public ChessColor Color;

            public Square( int rank, int file, PieceType type, ChessColor color )
            {
                Type = type;
                Color = color;
                Rank = rank;
                File = file;
            }

            public bool IsFree => Type == PieceType.Empty;
            public bool IsCapturable( ChessColor color ) => !IsFree && color != Color;
            public bool CanMoveTo( ChessColor color ) => IsFree || IsCapturable( color );
            public static Square Default => new( 0, 0, PieceType.Empty, ChessColor.White );
        }

        private List<List<Square>> _board;
        private int _height;
        private int _width;

        public ShallowBoard( int width, int height )
        {
            _board = new List<List<Square>>(); // dummy so we can use 1-indexed
            _height = height;
            _width = width;

            for ( int i = 1; i <= _height + 1; i++ )
            {
                _board.Add( new() ); // dummy so we can use 1-indexed
                for ( int j = 1; j <= _width + 1; j++ )
                {
                    _board[ i ].Add( Square.Default );
                }
            }
        }

        public ShallowBoard( List<List<Square>> board )
        {
            _board = board;
        }

        public bool OutOfBounds( int rank, int file ) => rank < 1 || file < 1 || rank > _height || file > _width;

        public Square GetSquare( int rank, int file ) => _board[ rank ][ file ];

        public void SetSquare( int rank, int file, PieceType type, ChessColor color )
        {
            SetSquare( rank, file, new( rank, file, type, color ) );
        }

        public void SetSquare( int rank, int file, Square square )
        {
            _board[ rank ][ file ] = square;
        }

        public void SwapSquares( int rank1, int file1, int rank2, int file2 )
        {
            (_board[ rank1 ][ file1 ], _board[ rank2 ][ file2 ]) = (_board[ rank2 ][ file2 ], _board[ rank1 ][ file1 ]);
        }

        // 1 captures 2
        public void CaptureSquare( int rank1, int file1, int rank2, int file2 )
        {
            _board[ rank2 ][ file2 ] = _board[ rank1 ][ file1 ];
            _board[ rank1 ][ file1 ] = Square.Default;
        }

        private Square FindKing( ChessColor color )
        {
            for ( int rank = 1; rank <= _height; ++rank )
            {
                for ( int file = 1; file <= _width; ++file )
                {
                    Square square = _board[ rank ][ file ];
                    if ( square.Type == PieceType.King && square.Color == color )
                    {
                        return square;
                    }
                }
            }
            // will not be reached
            return Square.Default;
        }

        private List<Square> CheckInDirection( Square kingSquare, int[] direction, List<PieceType> validPieces )
        {
            List<Square> ret = new();
            (int kingRank, int kingFile) = (kingSquare.Rank, kingSquare.File);
            for ( int i = 0; i < direction.Length - 1; ++i )
            {
                Square square = Utilities.GetPieceInDirection( this,
                    kingRank, kingFile, kingSquare.Color,
                    direction[ i ], direction[ i + 1 ] );
                if ( square == Square.Default )
                {
                    continue;
                }
                // a piece that could be giving check
                if ( validPieces.Contains( square.Type ) )
                {
                    ret.Add( square );
                }
            }
            return ret;
        }

        /// <summary>
        /// Check if the board is in a state of check or checkmate
        /// </summary>
        /// <param name="kingColor">The color of the king that may be in check</param>
        /// <returns></returns>
        public (bool isCheck, bool isCheckmate) LookForChecks( ChessColor kingColor )
        {
            // analyze color opposite to the one that is moving

            // find opposing king

            Square kingSquare = FindKing( kingColor );
            (int kingRank, int kingFile) = (kingSquare.Rank, kingSquare.File);
            List<Square> checkSquares = new();

            // check all diagonals
            checkSquares.AddRange(
                CheckInDirection( kingSquare, Utilities.DiagonalMoves, new() { PieceType.Bishop, PieceType.Queen } ) );

            // check all straights
            checkSquares.AddRange( 
                CheckInDirection( kingSquare, Utilities.StraightMoves, new() { PieceType.Rook, PieceType.Queen } ));
            
            // check all knight moves
            int[] knightMoves = Utilities.KnightMoves;

            for ( int i = 0; i < knightMoves.Length - 1; ++i )
            {
                Square square = GetSquare(kingRank + knightMoves[i], kingFile + knightMoves[i + 1]);
                if ( square == Square.Default )
                {
                    continue;
                }
                // a piece that could be giving check
                if ( square.IsCapturable( kingColor ) && square.Type == PieceType.Knight )
                {
                    checkSquares.Add( square );
                }
            }

            // we have all squares that could be giving check

            Assert.IsTrue( checkSquares.Count <= 2 );


            return (false, false);
        }
    }
}
