using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Misc
{

    public static class Utilities
    {

        // x and y are co-efficients for what direction the piece is trying to move into
        public static List<Move> GetMovesInDirection( Board board, int rank, int file, ChessColor color, int x, int y )
        {
            List<Move> res = new();
            Square from = board.GetSquare( rank, file );
            int i = 1;
            while ( board.CanMoveTo( rank + i * x, file + i * y, color ) )
            {
                res.Add( MoveCreator.CreateMove( board, from, board.GetSquare( rank + i * x, file + i * y ) ) );
                if ( !board.IsFree( rank + i * x, file + i * y ) ) break;
                ++i;
            }
            return res;
        }

        // Get all valid rook moves from a square on the board
        public static List<Move> GetRookMoves( Board board, int rank, int file, ChessColor color )
        {
            List<Move> res = new();
            res.AddRange( GetMovesInDirection( board, rank, file, color, 1, 0 ) );
            res.AddRange( GetMovesInDirection( board, rank, file, color, -1, 0 ) );
            res.AddRange( GetMovesInDirection( board, rank, file, color, 0, 1 ) );
            res.AddRange( GetMovesInDirection( board, rank, file, color, 0, -1 ) );
            return res;
        }

        // Get all valid bishop moves from a square on the board
        public static List<Move> GetBishopMoves( Board board, int rank, int file, ChessColor color )
        {
            List<Move> res = new();
            res.AddRange( GetMovesInDirection( board, rank, file, color, 1, 1 ) );
            res.AddRange( GetMovesInDirection( board, rank, file, color, 1, -1 ) );
            res.AddRange( GetMovesInDirection( board, rank, file, color, -1, 1 ) );
            res.AddRange( GetMovesInDirection( board, rank, file, color, -1, -1 ) );
            return res;
        }

        public static List<Move> GetKnightMoves( Board board, int rank, int file, ChessColor color )
        {
            int[] moves = { 2, 1, -2, -1, 2, -1, -2, 1, 2 };
            List<Move> res = new();
            Square from = board.GetSquare( rank, file );

            for ( int i = 0; i < moves.Length - 1; ++i )
            {
                (int rankTo, int fileTo) = (rank + moves[ i ], file + moves[ i + 1 ]);

                if ( board.CanMoveTo( rankTo, fileTo, color ) )
                {
                    res.Add( MoveCreator.CreateMove( board, from, board.GetSquare( rankTo, fileTo ) ) );
                }
            }
            return res;
        }

        public static List<Move> GetKingMoves( Board board, int rank, int file, ChessColor color )
        {
            int[] moves = { 1, 0, -1, 0, 1, 0, -1, 0, 1 };
            List<Move> res = new();
            Square from = board.GetSquare( rank, file );

            for ( int i = 0; i < moves.Length - 1; ++i )
            {
                (int rankTo, int fileTo) = (rank + moves[ i ], file + moves[ i + 1 ]);

                if ( board.CanMoveTo( rankTo, fileTo, color ) )
                {
                    res.Add( MoveCreator.CreateMove( board, from, board.GetSquare( rankTo, fileTo ) ) );
                }
            }
            return res;
        }

        //vector<string> Split( string s, char delim )
        //{
        //    vector<string> ret;
        //    int prev = 0;
        //    size_t n = s.size();
        //    for ( int i = 0; i < n; ++i )
        //    {
        //        if ( s[ i ] == delim )
        //        {
        //            if ( prev == i )
        //            {
        //                prev = i + 1;
        //                continue;
        //            };
        //            ret.push_back( s.substr( prev, i - prev ) );
        //            prev = i + 1;

        //        }
        //    }
        //    if ( prev != n )
        //    {
        //        ret.push_back( s.substr( prev, n - prev ) );
        //    }
        //    return ret;
        //}

        public static (CRank rank, CFile file) ReadChessNotation( string s )
        {
            if ( s.Length != 2 )
            {
                throw new ArgumentException( "Invalid String Length for Chess Notation" );
            }

            int rankNum = s[ 1 ] - '0';
            int fileNum = s[ 0 ] - 'A' + 1;

            CRank rank = new( rankNum );
            CFile file = new( fileNum );


            // change these bounds if the board gets bigger
            if ( rankNum < 1 || rankNum > 8 || fileNum < 1 || fileNum > 8 )
            {
                throw new ArgumentException( "Invalid Position for Chess Notation" );
            }

            return (rank, file);
        }

    }
}