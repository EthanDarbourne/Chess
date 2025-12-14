using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Moves;
using Assets.Scripts.ShallowCopy;
using System;

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

        public override List<Move> GetPotentialMoves( Board board )
        {
            // check all valid moves for a king
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check king moves
            List<Move> res = board.GetKingMoves( rank, file, Color );
            Square from = board.GetSquare( Location );
            // check castling
            if ( !_hasMoved && !IsInCheck( board ) )
            {
                // check rooks
                //CustomLogger.LogDebug( "Checking castling" );
                int GetRookFileInDirection( int dir )
                {
                    int startFile = file;
                    while ( true )
                    {
                        if ( board.OutOfBounds( rank, startFile + dir ) ) return -1;
                        startFile += dir;
                        if ( !board.IsFree( rank, startFile ) ) break;
                    }
                    Square rookSquare = board.GetSquare( rank, startFile );
                    Piece? piece = rookSquare.Piece;
                    if ( piece is not null && piece is Rook rook && rook.HasntMoved )
                    {
                        return startFile;
                    }
                    return -1;
                }

                void GetCastleMoves( int rookFile, int rookEnd, int kingEnd )
                {
                    int fileToCheckTo;
                    if (rookFile < file)
                    {
                        fileToCheckTo = Math.Min(rookFile, rookEnd);
                    }
                    else
                    {
                        fileToCheckTo = Math.Max(rookFile, rookEnd);
                    }

                    // make sure there are no pieces between rook and king, or on spaces where we have to castle to
                    int dir = rookFile < file ? -1 : 1;
                    for(int fileI = file + dir; fileI != fileToCheckTo + dir; fileI += dir)
                    {
                        if (!board.IsFree(rank, fileI) && fileI != rookFile)
                        {
                            return;
                        }
                    }

                    // castling is possible, so long as we don't move through check
                    Square rookSquare = board.GetSquare(rank, rookFile);
                    res.Add(MoveCreator.CreateCastlingMove(board, from, rookSquare, rookSquare));

                    // if 2 or more squares between king and rook, click on that square to castle
                    if(Math.Abs(rookFile - file) - 1 >= 2)
                    {
                        Square to = board.GetSquare(rank, file + dir * 2);
                        res.Add(MoveCreator.CreateCastlingMove(board, from, to, rookSquare));
                    }
                }

                int rookFile = GetRookFileInDirection(1);
                if(rookFile != -1)
                {
                    GetCastleMoves(rookFile, Constants.CASTLING_KING_SIDE_ROOK_END, Constants.CASTLING_KING_SIDE_KING_END);
                }
                rookFile = GetRookFileInDirection(-1);
                if (rookFile != -1)
                {
                    GetCastleMoves(rookFile, Constants.CASTLING_QUEEN_SIDE_ROOK_END, Constants.CASTLING_QUEEN_SIDE_KING_END);
                }

                // no pieces between king and rook
                // respective squares are empty or occupied by this rook and king
                // to castle, click on rook or two squares away from king, if that square is free
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

        public override ShallowPiece CreateShallowPiece() =>
            new ShallowKing( _location.Rank.Num, _location.File.Num, Color );
    }
}
