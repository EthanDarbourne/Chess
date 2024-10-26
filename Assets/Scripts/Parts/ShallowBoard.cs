﻿using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
            public void LogInfo()
            {
                if ( this == Default )
                {
                    Debug.Log( "Default Square" );
                }
                else
                {
                    Debug.Log( $"Rank: {Rank}, File: {File}, Type:{Type}, Color:{Color}" );
                }
            }
        }

        private List<List<Square>> _board;
        private int _height;
        private int _width;
        private ChessColor _currentTurn;

        public ShallowBoard( int width, int height, ChessColor currentTurn = ChessColor.White )
        {
            _board = new List<List<Square>>() { null }; // dummy so we can use 1-indexed
            _height = height;
            _width = width;
            _currentTurn = currentTurn;

            for ( int i = 1; i <= _height; i++ )
            {
                _board.Add( new() { null } ); // dummy so we can use 1-indexed
                for ( int j = 1; j <= _width; j++ )
                {
                    _board[ i ].Add( Square.Default );
                }
            }
        }

        public ShallowBoard( List<List<Square>> board, ChessColor currentTurn = ChessColor.White )
        {
            _board = board;
            _currentTurn = currentTurn;
        }

        public ShallowBoard( ShallowBoard board ) : this( board._width, board._height, board._currentTurn )
        {
            for ( int i = 1; i <= _height; i++ )
            {
                for ( int j = 1; j <= _width; j++ )
                {
                    _board[ i ][ j ] = board._board[ i ][ j ];
                }
            }
        }

        public void OnMoveExecuted()
        {
            _currentTurn = Utilities.FlipTurn( _currentTurn );
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
            // todo: cleanup
            (_board[ rank1 ][ file1 ].Type, _board[ rank2 ][ file2 ].Type) =
                (_board[ rank2 ][ file2 ].Type, _board[ rank1 ][ file1 ].Type);
            (_board[ rank1 ][ file1 ].Color, _board[ rank2 ][ file2 ].Color) =
                (_board[ rank2 ][ file2 ].Color, _board[ rank1 ][ file1 ].Color);
        }

        // 1 captures 2
        public void CaptureSquare( int rank1, int file1, int rank2, int file2 )
        {
            // todo: cleanup
            _board[ rank2 ][ file2 ].Type = _board[ rank1 ][ file1 ].Type;
            _board[ rank2 ][ file2 ].Color = _board[ rank1 ][ file1 ].Color;
            _board[ rank1 ][ file1 ].Type = PieceType.Empty;
            _board[ rank1 ][ file1 ].Color = ChessColor.White;
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
            Assert.IsFalse( true );
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
            //Debug.Log( $"Trying {kingRank}, {kingFile}" );
            Debug.Assert( GetSquare( kingRank, kingFile ).Type == PieceType.King );
            for ( int i = 0; i < direction.Length - 1; ++i )
            {
                Square square = this.GetPieceInDirection(
                    kingRank, kingFile,
                    direction[ i ], direction[ i + 1 ] );
                //square.LogInfo();
                if ( square == Square.Default )
                {
                    continue;
                }
                // a piece that could be giving check
                if ( validPieces.Contains( square.Type ) )
                {
                    //Debug.Log( $"{kingRank},{kingFile} : {square.Color}. {kingSquare.Color}" );
                    Assert.IsFalse( square.Color == kingSquare.Color );
                    ret.Add( square );
                }
            }
            return ret;
        }

        // todo:
        // optimize by checking through opponent piece positions
        private bool KingCanMoveTo( Square kingSquare, Square newKingSquare )
        {
            Move move;
            if ( newKingSquare.IsCapturable( kingSquare.Color ) )
            {
                move = this.CreateShallowCaptureMove( kingSquare, newKingSquare );
            }
            else if ( newKingSquare.IsFree )
            {
                move = this.CreateShallowBasicMove( kingSquare, newKingSquare );
            }
            else
            {
                return false;
                //throw new Exception( "King cannot move to this square" );
            }
            var kingShallowBoard = new ShallowBoard( this );
            move.ExecuteShallowMove( kingShallowBoard );
            bool canMoveTo = true;
            canMoveTo &= kingShallowBoard.CheckInDirection( newKingSquare, Utilities.StraightMoves, Utilities.StraightPieceTypes ).Any();
            canMoveTo &= kingShallowBoard.CheckInDirection( newKingSquare, Utilities.DiagonalMoves, Utilities.DiagonalPieceTypes ).Any();
            canMoveTo &= kingShallowBoard.CheckOnSquare( newKingSquare, Utilities.KnightMoves, Utilities.KnightPieceTypes ).Any();
            canMoveTo &= newKingSquare.Color == ChessColor.Black &&
                    kingShallowBoard.CheckOnSquare( newKingSquare, Utilities.BlackPawnMoves, Utilities.PawnTypes ).Any();
            canMoveTo &= newKingSquare.Color == ChessColor.White &&
                    kingShallowBoard.CheckOnSquare( newKingSquare, Utilities.WhitePawnMoves, Utilities.PawnTypes ).Any();

            return canMoveTo;
        }

        /// <summary>
        /// Check if the board is in a state of check or checkmate
        /// </summary>
        /// <param name="kingColor">The color of the king that may be in check</param>
        /// <returns>true if there is a check/checkmate, and true if there is a checkmate</returns>
        public (bool isCheck, bool isCheckmate) LookForChecksOnKing( ChessColor kingColor )
        {
            // analyze color opposite to the one that is moving

            // find opposing king

            Square kingSquare = FindKing( kingColor );
            //Debug.Log( $"Found king at {kingSquare.Rank}, {kingSquare.File}" );
            (int kingRank, int kingFile) = (kingSquare.Rank, kingSquare.File);
            List<Square> checkSquares = new();

            // check all diagonals
            checkSquares.AddRange(
                CheckInDirection( kingSquare, Utilities.DiagonalMoves, Utilities.DiagonalPieceTypes ) );

            //Debug.Log( $"Count on diagonal = {checkSquares.Count}" );

            // check all straights
            checkSquares.AddRange(
                CheckInDirection( kingSquare, Utilities.StraightMoves, Utilities.StraightPieceTypes ) );

            //Debug.Log( $"Count on straights = {checkSquares.Count}" );
            // check all knight moves
            checkSquares.AddRange( CheckOnSquare( kingSquare, Utilities.KnightMoves, Utilities.KnightPieceTypes ) );

            //Debug.Log( $"Count on knights = {checkSquares.Count}" );
            // check pawns
            if ( kingSquare.Color == ChessColor.Black )
            {
                checkSquares.AddRange(
                    CheckOnSquare( kingSquare, Utilities.WhitePawnMoves, Utilities.PawnTypes ) );
            }
            else
            {
                checkSquares.AddRange(
                    CheckOnSquare( kingSquare, Utilities.BlackPawnMoves, Utilities.PawnTypes ) );
            }
            //Debug.Log( $"Count on pawns = {checkSquares.Count}" );
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
                Square checkSquare = checkSquares.First();
                IEnumerable<Square> blockableSquares = GetSquaresBetween( checkSquare, kingSquare );

                // for every piece, see if they can move to these squares, and it is a valid move
                IEnumerable<Square> pieces = GetPieces( kingColor ).Where( x => x.Type != PieceType.King );

                if ( blockableSquares.Any( x => pieces.Any( p => CanMovePieceToTarget( p, x ) ) ) )
                {
                    isCheckmate = false;
                }

                // for every piece, see if they can move to these squares and capture, and it is a valid move
                if ( pieces.Any( p => CanMovePieceToSquareAndCapture( p, checkSquare ) ) )
                {
                    isCheckmate = false;
                }
            }

            // see if king can move
            int[] kingMoves = Utilities.AdjacentMoves;
            for ( int i = 0; i < kingMoves.Length - 1; ++i )
            {
                if ( OutOfBounds( kingRank + kingMoves[ i ], kingFile + kingMoves[ i + 1 ] ) ) continue;
                Square square = GetSquare( kingRank + kingMoves[ i ], kingFile + kingMoves[ i + 1 ] );
                if ( KingCanMoveTo( kingSquare, square ) )
                {
                    isCheckmate = false;
                }
            }

            return (isCheck, isCheckmate);
        }

        private IEnumerable<Square> GetSquaresBetween( Square square1, Square square2 )
        {
            int diff1 = square2.Rank - square1.Rank;
            int diff2 = square2.File - square1.File;
            if ( Math.Abs( diff1 ) - Math.Abs( diff2 ) != 0 )
            {
                return Enumerable.Empty<Square>();
            }
            int steps = Math.Abs( diff1 );
            (int coeff1, int coeff2) = (diff1 / steps, diff2 / steps);
            return Enumerable.Range( 1, steps - 2 )
                .Select( x => GetSquare( square1.Rank + x * coeff1, square1.File + x * coeff2 ) );
        }

        private IEnumerable<Square> GetPieces( ChessColor color )
        {
            return _board.GetRange( 1, _height )
                .Select( x => x.GetRange( 1, _width ) )
                .SelectMany( x => x )
                .Where( x => x.Color == color && x.Type != PieceType.Empty );
        }

        private bool CanMovePieceToSquare( Square piece, Square target )
        {
            int diff1 = Math.Abs( piece.Rank - target.Rank );
            int diff2 = Math.Abs( piece.File - target.File );
            if ( diff1 < diff2 )
            {
                (diff1, diff2) = (diff2, diff1);
            }

            bool CanPawnMoveToSquare()
            {
                diff1 == 0 && ( diff2 == 0 || ( diff2 == 2 && ( piece.Rank == 2 && piece.Color == ChessColor.White ) || ( piece.Rank == 7 && piece.Color == ChessColor.Black ) ) )
                    || diff1 == 1
            }

            return piece.Type switch
            {
                PieceType.Knight => diff1 == 1 && diff2 == 2,
                // pawn moves in straight line, 
                PieceType.Pawn => diff1 == 0 && ( diff2 == 0 || ( diff2 == 2 && ( piece.Rank == 2 && piece.Color == ChessColor.White ) || ( piece.Rank == 7 && piece.Color == ChessColor.Black ) ) ),
                PieceType.Queen =>
            };
            // knights need 2,1
            // rooks need 1,0
            // bishops need 1,1
            // queens need 1,0 or 1,1 x == 1 && y == 0 or 1 ) or x = 0 & y = 1
        }

        // try to move piece on first square to the target square
        private bool CanMovePieceToTarget( Square piece, Square target )
        {
            // target square is always empty
            // need to try a basic move to get there
            ShallowBasicMove move = MoveCreator.CreateShallowBasicMove( this, piece, target );

            ShallowBoard copy = new( this );
            move.ExecuteShallowMove( copy );
            return copy.IsValidPosition();
        }

        private bool CanMovePieceToSquareAndCapture( Square piece, Square target )
        {
            // target square is always empty
            // need to try a basic move to get there
            ShallowCaptureMove move = MoveCreator.CreateShallowCaptureMove( this, piece, target );

            ShallowBoard copy = new( this );
            move.ExecuteShallowMove( copy );
            return copy.IsValidPosition();
        }

        public bool IsValidPosition()
        {
            // if current turn is in check, move is invalid
            ChessColor turn = Utilities.FlipTurn( _currentTurn );
            (bool isCheck, _) = LookForChecksOnKing( turn );
            return !isCheck;
        }
    }
}
