using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class Board
    {
        // find a way to make the enum bigger to allow for bigger boards (use more letters)

        private List<List<Square>> _board = new();

        private Piece? _selectedPiece = null;

        private readonly HighlightSquare _highlightSquare;

        private readonly int _width;
        private readonly int _height;

        private ChessColor _turn = ChessColor.White;

        public Board( int width, int height, GameObject boardObject, HighlightSquare highlightSquare )
        {
            _width = width;
            _height = height;
            _highlightSquare = highlightSquare;
            Reset();
        }

        // get the width of the board
        public int Width => _width;

        // get the height of the board
        public int Height => _height;

        public void SetupForGameStart()
        {
            DeselectPiece();
            DisableAllHighlights();
        }

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
                Debug.Log( "We have a selected piece: " + _selectedPiece.Type.ToString() );
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

        // if we have already selected a piece, we try and move the selected piece to the selected location
        // otherwise, we deselect the current piece 
        private void TryToMoveSelectedPieceToLocation( int rank, int file )
        {
            List<Square> moves = _selectedPiece.GetValidMoves( this );

            Square? moveSquare = moves.FirstOrDefault( x => x.Rank.Num == rank && x.File.Num == file );

            if(moveSquare is null)
            {
                Debug.Log( "Deselecting piece" );

                bool shouldSelectAnotherPiece = _selectedPiece.Location.Rank.Num != rank || _selectedPiece.Location.File.Num != file;
                DeselectPiece();
                // try and select the piece at this square, if we can
                if( shouldSelectAnotherPiece )
                {
                    SelectPiece( rank, file );
                }
                return;
            }
            MovePiece( GetSquare( _selectedPiece.Location ), moveSquare );
        }

        // try to select the piece at the current location
        private void SelectPiece(int rank, int file)
        {
            Debug.Log( $"Trying to select at {rank}, {file}" );
            Piece? piece = _board[ rank ][ file ].Piece;
            if ( piece is null )
            {
                Debug.Log( "There is no piece on this square" );
                return;
            }

            if ( piece.Color != _turn )
            {
                Debug.Log( "This is not one of your pieces" );
                return;
            }

            HighlightSquare( _highlightSquare, rank, file );
            _selectedPiece = piece;

            List<Square> moves = piece.GetValidMoves( this );

            foreach ( Square move in moves )
            {
                move.EnableMoveToHighlight();
            }
        }

        public Square GetSquare( int rank, int file )
        {
            if ( OutOfBounds( rank, file ) )
            {
                return null; // todo throw exception
            }
            return _board[ rank ][ file ];
        }

        public Square GetSquare( CRank rank, CFile file ) => GetSquare( rank.Num, file.Num );

        public Square GetSquare( Point point ) => GetSquare( point.Rank, point.File );

        public Square GetSquare( string s )
        {
            (CRank rank, CFile file) = Utilities.ReadChessNotation( s );
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

        //// takes in the rank 0-indexed and char representing file 
        //public void SetPiece( int rank, char file, char Pcode, ChessColor color )
        //{
        //    int fileIdx = file - 'A' + 1;
        //    switch ( Pcode )
        //    {
        //        case 'p':
        //            _board[ rank ][ fileIdx ].SetPiece( new Pawn( color ) );
        //            break;
        //        case 'R':
        //            _board[ rank ][ fileIdx ].SetPiece( new Rook( color ) );
        //            break;
        //        case 'N':
        //            _board[ rank ][ fileIdx ].SetPiece( new Knight( color ) );
        //            break;
        //        case 'B':
        //            _board[ rank ][ fileIdx ].SetPiece( new Bishop( color ) );
        //            break;
        //        case 'Q':
        //            _board[ rank ][ fileIdx ].SetPiece( new Queen( color ) );
        //            break;
        //        case 'K':
        //            _board[ rank ][ fileIdx ].SetPiece( new King( color ) );
        //            break;
        //    }
        //}

        // build the default board
        public void Reset()
        {
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

        public void MovePiece( int rankfrom, int filefrom, int rankto, int fileto )
        {
            MovePiece( _board[ rankfrom ][ filefrom ], _board[ rankto ][ fileto ] );
            //swap with tuple expression
            //( _board[ rankfrom ][ filefrom ], _board[ rankto ][ fileto ]) = (_board[ rankto ][ fileto ], _board[ rankfrom ][ filefrom ]);
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
                Debug.Log( "No piece on this square" );
                return;
            }

            List<Square> moves = from.Piece.GetValidMoves( this );

            if ( moves.Find( x => x.Point == to.Point ) is null )
            {
                Debug.Log( "Cannot move to this location" );
                return;
            }

            Piece movingPiece = from.RemovePiece();
            
            if(to.IsCapturable(_turn))
            {
                // making capture
                // todo: move to side of board
                to.CapturePiece( movingPiece );
            } 
            else
            {
                to.MovePieceTo( movingPiece );
            }
            DeselectPiece();
            SwapTurn();
        }

        public void MovePiece( string from, string to )
        {
            MovePiece( GetSquare( from ), GetSquare( to ) );
        }

        public void SwapTurn()
        {
            _turn = _turn == ChessColor.White ? ChessColor.Black : ChessColor.White;
        }
    }
}