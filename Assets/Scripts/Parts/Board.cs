﻿using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Parts
{
    public class Board
    {
        // find a way to make the enum bigger to allow for bigger boards (use more letters)

        private List<List<Square>> _board = new();
        private readonly List<Move> _moves = new();

        private int _currentMove = 0;

        private Piece? _selectedPiece = null;

        private readonly HighlightSquare _highlightSquare;
        private readonly PromotionSelector _promotionSelector;
        private readonly PieceManager _pieceManager;

        private readonly int _width;
        private readonly int _height;

        private ChessColor _turn = ChessColor.White;

        #region InitialBuild

        public Board( int width, int height, GameObject boardObject, HighlightSquare highlightSquare )
        {
            _width = width;
            _height = height;
            _highlightSquare = highlightSquare;
            BuildBoard();
        }

        public Board( int width, int height, PieceManager pieceManager, HighlightSquare highlightSquare, PromotionSelector selector )
        {
            _width = width;
            _height = height;
            _highlightSquare = highlightSquare;
            _pieceManager = pieceManager;
            _promotionSelector = selector;
            //BuildBoard();
        }

        // build the default board
        public void BuildBoard()
        {
            _board.Skip(1).SelectMany( x => x ).Where(x => x is not null).ToList().ForEach( x => x.Destroy() );
            _board.Clear();
            _board = new List<List<Square>> { null }; // dummy so we can use 1-indexed

            for ( int i = 1; i < _height + 1; i++ )
            {
                _board.Add( new() { null } ); // dummy so we can use 1-indexed
                for ( int j = 1; j < _width + 1; j++ )
                {
                    _board[ i ].Add( new Square( new Point( i, j ) ) );
                }
            }
        }

        public void SetupForGameStart()
        {
            MovePiecesToStartSquares();
            DeselectPiece();
            DisableAllHighlights();
        }

        private void MovePiecesToStartSquares()
        {
            if ( _pieceManager is null )
            {
                return;
            }
            BuildBoard();
            foreach ( var pieceNotation in Constants.InitialPiecePositions )
            {
                char type = pieceNotation.Length == 2 ? 'p' : pieceNotation[ 0 ];
                string position = pieceNotation[ ^2.. ];
                ChessColor color = position[1] == '1' || position[1] == '2' ? ChessColor.White : ChessColor.Black;
                PieceType pieceType = Utilities.GetPieceType( type );
                Piece piece = GeneratePiece( pieceType, color );
                (CRank rank, CFile file) = Utilities.ReadChessNotation( position );
                SetPiece( rank, file, piece );
            }
        }

        private Piece GeneratePiece(PieceType type, ChessColor color)
        {
            GameObject pieceObject = _pieceManager.GeneratePiece( type, color );
            return Utilities.CreatePiece( type, color, pieceObject );
        }

        #endregion InitialBuild

        // get the width of the board
        public int Width => _width;

        // get the height of the board
        public int Height => _height;

        public ChessColor Turn => _turn;

        public Move LastMove => _moves.Count > 0 ? _moves[ ^1 ] : null;
        public int MoveCount => _moves.Count;

        public bool CanMoveForward => _currentMove < _moves.Count;
        public bool CanMoveBackward => _currentMove > 0;



        public void SelectPiece( CRank rank, CFile file )
        {
            SelectPiece( rank.Num, file.Num );
        }

        public void SelectPiece( Square? sq )
        {
            if ( sq is null ) return;
            Point point = sq.Point;
            SelectPiece( point.Rank, point.File );
        }

        public void SelectPiece( string s )
        {
            Square square = GetSquare( s );
            SelectPiece( square );
        }

        public void SelectLocation( int rank, int file )
        {

            if ( _selectedPiece is not null )
            {
                TryToMoveSelectedPieceToLocation( rank, file );
            }
            else
            {
                SelectPiece( rank, file );
            }
        }

        public void DeselectPiece()
        {
            DisableAllHighlights();
            _selectedPiece = null;
        }

        public void SelectPieceCoroutine()
        {

        }

        // if we have already selected a piece, we try and move the selected piece to the selected location
        // otherwise, we deselect the current piece 
        private IEnumerator TryToMoveSelectedPieceToLocation( int rank, int file )
        {
            IEnumerable<Move> moves = _selectedPiece.GetValidMoves( this );

            moves = moves.Where( x => x.To.Rank.Num == rank && x.To.File.Num == file );

            if(moves.Count() == 0)
            {
                bool shouldSelectAnotherPiece = _selectedPiece.Location.Rank.Num != rank || _selectedPiece.Location.File.Num != file;
                DeselectPiece();
                // try and select the piece at this square, if we can
                if ( shouldSelectAnotherPiece )
                {
                    SelectPiece( rank, file );
                }
            }
            else if(moves.Count() == 1)
            {
                MovePiece( GetSquare( _selectedPiece.Location ), moves.First().To );
            }
            else
            {
                Debug.Log( "Promoting" );
                Vector3 promotionPosition = _selectedPiece.Location.Vector;
                promotionPosition.y = 3;
                _promotionSelector.Display( promotionPosition );
                yield return _promotionSelector.WaitForSelection();

                PieceType promoteTo = _promotionSelector.LastSelectedPiece;
                Move move = moves.First( x => ( x as IPromotionMove ).PromoteTo == promoteTo );
                MovePiece( move );
            }
            yield break;
        }

        // try to select the piece at the current location
        private void SelectPiece( int rank, int file )
        {
            Piece? piece = _board[ rank ][ file ].Piece;
            if ( piece is null )
            {
                //Debug.Log( "There is no piece on this square" );
                return;
            }

            if ( piece.Color != _turn )
            {
                //Debug.Log( "This is not one of your pieces" );
                return;
            }

            HighlightSquare( _highlightSquare, rank, file );
            _selectedPiece = piece;

            List<Move> moves = piece.GetValidMoves( this );

            Debug.Log( $"Start possible moves: {moves.Count}" );
            foreach ( Move move in moves )
            {
                Debug.Log( $"Move: {Utilities.PrintNotation( move.From.Rank.Num, move.From.File.Num )}, {Utilities.PrintNotation( move.To.Rank.Num, move.To.File.Num )}" );
                move.EnableMoveToHighlight();
            }
        }

        public Square? GetSquareOrDefault( int rank, int file )
        {
            if ( OutOfBounds( rank, file ) )
            {
                return null;
            }
            return _board[ rank ][ file ];
        }

        public Square GetSquare( int rank, int file )
        {
            if ( OutOfBounds( rank, file ) )
            {
                throw new Exception( $"Out of bounds at rank: {rank}, file: {file}" );
            }
            return _board[ rank ][ file ];
        }

        public Square GetSquare( CRank rank, CFile file ) => GetSquare( rank.Num, file.Num );

        public Square GetSquare( Point point ) => GetSquare( point.Rank, point.File );

        public Square GetSquare( string s )
        {
            (CRank rank, CFile file) = s.ReadChessNotation();
            Square ret = GetSquare( rank.Num, file.Num );
            return ret;
        }

        public bool OutOfBounds( CRank rank, CFile file ) => OutOfBounds( rank.Num, file.Num );

        public bool OutOfBounds( int rank, int file ) => rank <= 0 || rank > _height || file <= 0 || file > _width;

        public bool CanMoveTo( CRank rank, CFile file, ChessColor color ) => CanMoveTo( rank.Num, file.Num, color );

        public bool CanMoveTo( int rank, int file, ChessColor color ) => !OutOfBounds( rank, file ) && _board[ rank ][ file ].IsFreeOrCapturable( color );

        public bool CanCapture( int rank, int file, ChessColor color ) => !OutOfBounds( rank, file ) && _board[ rank ][ file ].IsCapturable( color );

        public bool CanCapture( CRank rank, CFile file, ChessColor color ) => CanCapture( rank.Num, file.Num, color );

        public bool IsEnemyPiece( CRank rank, CFile file, ChessColor color ) => IsEnemyPiece( rank.Num, file.Num, color );

        public bool IsEnemyPiece( int rank, int file, ChessColor color )
        {
            // check bounds
            if ( OutOfBounds( rank, file ) ) return false;

            Piece piece = _board[ rank ][ file ].Piece;
            if ( piece is null )
            {
                return false;
            }
            return piece.Color != color;
        }

        public bool IsFree( CRank rank, CFile file ) => IsFree( rank.Num, file.Num );

        public bool IsFree( int rank, int file ) => !OutOfBounds( rank, file ) && _board[ rank ][ file ].IsFree;


        public void SetPiece( CRank rank, CFile file, Piece piece )
        {
            SetPiece( rank.Num, file.Num, piece );
        }

        public void SetPiece( int rank, int file, Piece piece )
        {
            _board[ rank ][ file ].SetPiece( piece );
        }

        public void HighlightSquare( HighlightSquare highlight, CRank rank, CFile file )
        {
            HighlightSquare( highlight, rank.Num, file.Num );
        }

        public void HighlightSquare( HighlightSquare highlight, int rank, int file )
        {
            _board[ rank ][ file ].HighlightSquare( highlight );
        }

        public void DisableAllHighlights()
        {
            DisableAllMoveToHighlights();
            _highlightSquare.Hide();
        }

        public void DisableAllMoveToHighlights()
        {
            _board.ForEach( x => x?.ForEach( y => y?.DisableMoveToHighlight() ) );
        }

        public void MovePiece( int rankfrom, int filefrom, int rankto, int fileto )
        {
            MovePiece( _board[ rankfrom ][ filefrom ], _board[ rankto ][ fileto ] );
        }

        public void MovePiece( CRank rankfrom, CFile filefrom, CRank rankto, CFile fileto )
        {
            MovePiece( rankfrom.Num, filefrom.Num, rankto.Num, fileto.Num );
        }

        public void MovePiece( Square from, Square to )
        {
            if ( OutOfBounds( from.Rank, from.File ) || OutOfBounds( to.Rank, to.File ) )
            {
                return;
            }
            if ( IsFree( from.Rank, from.File ) )
            {
                //Debug.Log( "No piece on this square" );
                return;
            }

            List<Move> moves = from.Piece.GetValidMoves( this );

            List<Move> move = moves.Where( x => x.To.Point == to.Point ).ToList();

            if ( !move.Any() )
            {
                //Debug.Log( "Cannot move to this location" );
                return;
            }
            if(move.Count == 1)
            {
                MovePiece( move.Single() );
            }
            else
            {
                // have promotion moves
                // find out what to promote to

            }
        }

        public void MovePiece( Move move )
        {
            move.ExecuteMove( this );
            _moves.Add( move );
            ++_currentMove;
            DeselectPiece();
            (bool isCheck, _) = LookForChecks( _turn );
            //if( isCheck )
            //{
            //    HighlightKing( isCheck );
            //}
            SwapTurn();
        }

        public void MovePiece( string from, string to )
        {
            MovePiece( GetSquare( from ), GetSquare( to ) );
        }

        public bool IsValidPositionAfterMove( Move move )
        {
            ShallowBoard shallowBoard = GetShallowBoard();
            //(bool isCheck, bool isCheckmate) = shallowBoard.LookForChecksOnKing( _turn );
            //if ( isCheck )
            //{/

            //}
            move.ExecuteShallowMove( shallowBoard );
            return shallowBoard.IsValidPosition();
        }

        public void SwapTurn()
        {
            _turn = Utilities.FlipTurn( _turn );
        }

        public void Promote(Square square, PieceType type)
        {
            Assert.IsTrue( square.Rank.Num == 1 || square.Rank.Num == 8 );
            Assert.AreEqual( square.Piece?.Type, PieceType.Pawn );

            Piece promotedPiece = GeneratePiece( type, square.Piece.Color );
            promotedPiece.SetLocation( square.Point );
            square.CapturePiece( promotedPiece );
        }

        public void Unpromote( Square square )
        {
            Assert.IsTrue( square.Rank.Num == 1 || square.Rank.Num == 8 );
            Assert.IsNotNull( square.Piece );
            Assert.AreNotEqual( square.Piece?.Type, PieceType.Pawn );

            Piece pawn = GeneratePiece( PieceType.Pawn, square.Piece.Color );
            pawn.SetLocation( square.Point );
            square.CapturePiece( pawn );
        }

        public void UseCapturedPiece( Piece capturedPiece )
        {
            // move to side of board
        }

        // might happen implicitly
        //public void ReleaseCapturedPiece(Piece capturedPiece)

        // for moving between current moves that have been played
        public void ExecuteAllMoves()
        {
            while ( CanMoveForward )
            {
                ExecuteOneMove();
            }
        }

        public void UndoAllMoves()
        {
            while ( CanMoveBackward )
            {
                UndoOneMove();
            }
        }

        public void UndoOneMove()
        {
            if ( !CanMoveBackward ) return;
            --_currentMove;
            _moves[ _currentMove ].UndoMove( this );
        }

        public void ExecuteOneMove()
        {
            if ( !CanMoveForward ) return;
            _moves[ _currentMove ].ExecuteMove( this );
            ++_currentMove;
        }

        public ShallowBoard GetShallowBoard()
        {
            var shallowBoard = new ShallowBoard( Width, Height, _turn, LastMove );
            for ( int rank = 1; rank <= Height; ++rank )
            {
                for ( int file = 1; file <= Width; ++file )
                {
                    Piece? piece = GetSquare( rank, file ).Piece;
                    ShallowBoard.Square square =
                        new( rank, file, piece?.CreateShallowPiece() );
                    shallowBoard.SetSquare( rank, file, square );
                }
            }
            return shallowBoard;
        }

        public (bool isCheck, bool isCheckmate) LookForChecks( ChessColor kingColor )
        {
            var shallowBoard = GetShallowBoard();
            return shallowBoard.LookForChecksOnKing( kingColor );
        }

        public void DebugLog()
        {
            for ( int rank = 1; rank <= Height; ++rank )
            {
                string output = "";
                for ( int file = 1; file <= Width; ++file )
                {
                    output += _board[ rank ][ file ].Piece?.Type.GetChar() ?? ' ';
                }
                Debug.Log( output );
            }
        }
    }
}