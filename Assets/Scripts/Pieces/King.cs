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
        public King( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public King( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {
        }
        public override PieceType Type => PieceType.King;

        public override List<Square> GetValidMoves( Board board )
        {
            // check all valid moves for a pawn
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check bishop moves
            List<Square> res = Utilities.GetKnightMoves( board, rank, file, Color );

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
                    List<Square> moves = Utilities.GetMovesInDirection( board, rank, file, Color, 1, 0 );
                    Piece piece = moves[ -1 ].Piece;

                    if ( moves.Count == 0 || piece is null )
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
