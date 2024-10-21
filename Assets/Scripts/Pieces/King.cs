using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System;
using static UnityEditor.FilePathAttribute;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Moves;

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

        protected override List<Move> GetPotentialMoves( Board board )
        {
            // check all valid moves for a king
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check king moves
            List<Move> res = board.GetKingMoves( rank, file, Color );
            Square from = board.GetSquare( Location );
            //Debug.Log( "Got some king moves" );
            // check castling
            if ( !_hasMoved && !IsInCheck( board ) )
            {
                // check rooks
                // go left
                //Debug.Log( "Checking castling" );
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
                        //Debug.Log( "Found rook on rook square" );
                        int startFileMovable = file + dir * 2;
                        while(startFileMovable <= startFile )
                        {
                            Square to = board.GetSquare( rank, startFileMovable );
                            res.Add( MoveCreator.CreateCastlingMove( board, from, to, rookSquare ) );
                            ++startFileMovable;
                        }
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
                    List<Move> moves = board.GetMovesInDirection( rank, file, Color, j, k );
                    Piece? piece = moves.Count == 0 ? null : moves[ ^1 ].To.Piece;

                    if ( piece is null )
                    {
                        continue;
                    }
                    if ( (j + k) % 2 == 0 )
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
