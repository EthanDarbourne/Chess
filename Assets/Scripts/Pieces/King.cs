using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System;
using static UnityEditor.FilePathAttribute;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class King : Piece
    {
        private bool _hasMoved = false;

        public King( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public King( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {
        }

        public override PieceType Type => PieceType.King;

        public override void Move( int rankChange, int fileChange )
        {
            _hasMoved = true;
            base.Move( rankChange, fileChange );
        }

        public override List<Square> GetValidMoves( Board board )
        {
            // check all valid moves for a king
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check king moves
            List<Square> res = Utilities.GetKingMoves( board, rank, file, Color );
            Debug.Log( "Got some king moves" );
            // check castling
            if ( !_hasMoved && !IsInCheck( board ) )
            {
                // check rooks
                // go left
                Debug.Log( "Checking castling" );
                void GetCastleMoves( int dir )
                {
                    int startFile = file;
                    while ( true )
                    {
                        if ( board.OutOfBounds( rank, startFile + dir ) ) break;
                        startFile += dir;
                        if ( board.IsFree( rank, startFile ) ) continue;
                    }
                    Square rookSquare = board.GetSquare( rank, startFile );
                    Piece? piece = rookSquare.Piece;
                    if ( piece is not null && piece is Rook rook && rook.HasntMoved )
                    {
                        Debug.Log( "Found rook on rook square" );
                        res.Add( rookSquare );
                    }
                }

                GetCastleMoves( 1 );
                GetCastleMoves( -1 );

            }

            return res;
        }

        public bool IsInCheck( Board board )
        {
            int file = _location.File.Num;
            int rank = _location.Rank.Num;

            // check all rook, queen, and bishop checks
            for ( int j = -1; j <= 1; ++j )
            {
                for ( int k = -1; k <= 1; ++k )
                {
                    if ( j == 0 && k == 0 ) continue;
                    List<Square> moves = Utilities.GetMovesInDirection( board, rank, file, Color, j, k );
                    Piece? piece = moves.Count == 0 ? null : moves[ ^1 ].Piece;

                    if ( piece is null )
                    {
                        continue;
                    }
                    if ( j + k == 0 )
                    {
                        // bishop or queen can give check (diagonal)
                        if ( piece.Color != Color )
                        {
                            if ( piece.Type is PieceType.Queen or PieceType.Bishop )
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        // rook or queen can give check (horizontal)
                        if ( piece.Color != Color )
                        {
                            if ( piece.Type is PieceType.Queen or PieceType.Rook )
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            List<int> move = new() { 2, 2, -1, 1, -2, -2, 1, -1 };

            for ( int i = 0; i < 8; ++i )
            {
                if ( board.IsEnemyPiece( rank + move[ i ], file + move[ ( i + 2 ) % 8 ], Color ) )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
