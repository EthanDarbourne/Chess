using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

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
        private ChessColor _nextTurn;

        public ShallowBoard( int width, int height, ChessColor nextTurn = ChessColor.White )
        {
            _board = new List<List<Square>>() { null }; // dummy so we can use 1-indexed
            _height = height;
            _width = width;
            _nextTurn = nextTurn;

            for ( int i = 1; i <= _height + 1; i++ )
            {
                _board.Add( new() { null }  ); // dummy so we can use 1-indexed
                for ( int j = 1; j <= _width + 1; j++ )
                {
                    _board[ i ].Add( Square.Default );
                }
            }
        }

        public ShallowBoard( List<List<Square>> board, ChessColor nextTurn = ChessColor.White )
        {
            _board = board;
            _nextTurn = nextTurn;
        }

        public void OnMoveExecuted()
        {
            _nextTurn = _nextTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;
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

        private List<Square> CheckOnSquare( Square kingSquare, int[] direction, PieceType[] validPieces )
        {
            List<Square> ret = new();
            (int kingRank, int kingFile) = (kingSquare.Rank, kingSquare.File);
            for ( int i = 0; i < direction.Length - 1; ++i )
            {
                if ( OutOfBounds( kingRank + direction[ i ], kingFile + direction[ i + 1 ] ) ) continue;
                Square square = GetSquare( kingRank + direction[ i ], kingFile + direction[ i + 1 ] );
                if ( square == Square.Default )
                {
                    continue;
                }
                // a piece that could be giving check
                if ( validPieces.Contains( square.Type ) && square.Color != kingSquare.Color )
                {
                    ret.Add( square );
                }
            }
            return ret;
        }

        private List<Square> CheckInDirection( Square kingSquare, int[] direction, PieceType[] validPieces )
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
                    Assert.IsFalse( square.Color == kingSquare.Color );
                    ret.Add( square );
                }
            }
            return ret;
        }

        // todo:
        // optimize by checking through opponent piece positions
        private bool KingCanMoveTo( Square kingSquare )
        {

            bool canMoveTo = true;
            canMoveTo &= CheckInDirection( kingSquare, Utilities.StraightMoves, Utilities.StraightPieceTypes ).Any();
            canMoveTo &= CheckInDirection( kingSquare, Utilities.DiagonalMoves, Utilities.DiagonalPieceTypes ).Any();
            canMoveTo &= CheckOnSquare( kingSquare, Utilities.KnightMoves, Utilities.KnightPieceTypes ).Any();
            canMoveTo &= kingSquare.Color == ChessColor.Black &&
                    CheckOnSquare( kingSquare, Utilities.BlackPawnMoves, Utilities.PawnTypes ).Any();
            canMoveTo &= kingSquare.Color == ChessColor.White &&
                    CheckOnSquare( kingSquare, Utilities.WhitePawnMoves, Utilities.PawnTypes ).Any();
            return canMoveTo;
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
                CheckInDirection( kingSquare, Utilities.DiagonalMoves, Utilities.DiagonalPieceTypes ) );

            // check all straights
            checkSquares.AddRange(
                CheckInDirection( kingSquare, Utilities.StraightMoves, Utilities.StraightPieceTypes ) );

            // check all knight moves
            checkSquares.AddRange( CheckOnSquare( kingSquare, Utilities.KnightMoves, Utilities.KnightPieceTypes ) );

            // check pawns
            if(kingSquare.Color == ChessColor.Black)
            {
                checkSquares.AddRange(
                    CheckOnSquare( kingSquare, Utilities.WhitePawnMoves, Utilities.PawnTypes ) );
            }
            else
            {
                checkSquares.AddRange(
                    CheckOnSquare( kingSquare, Utilities.BlackPawnMoves, Utilities.PawnTypes ) );
            }

            // we have all squares that could be giving check
            Assert.IsTrue( checkSquares.Count <= 2 );

            if ( !checkSquares.Any() )
            {
                return (false, false);
            }

            // if two pieces giving check, king must move
            (bool isCheck, bool isCheckmate) = (true, true);

            if ( checkSquares.Count == 1 )
            {
                // see if check can be captured/blocked
            }

            // see if king can move
            int[] kingMoves = Utilities.AdjacentMoves;
            for ( int i = 0; i < kingMoves.Length - 1; ++i )
            {
                if ( OutOfBounds( kingRank + kingMoves[ i ], kingFile + kingMoves[ i + 1 ] ) ) continue;
                Square square = GetSquare( kingRank + kingMoves[ i ], kingFile + kingMoves[ i + 1 ] );
                if ( KingCanMoveTo( square ) )
                {
                    isCheckmate = false;
                }
            }

            return (isCheck, isCheckmate);
        }

        public bool IsValidPosition()
        {
            // if current turn is in check, move is invalid
            (bool isCheck, _) = LookForChecks( _nextTurn );
            return !isCheck;
        }
    }
}
