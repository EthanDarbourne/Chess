using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using Assets.Scripts.ShallowCopy;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc
{

    public static class Utilities
    {
        public static int[] StraightMoves = { -1, 0, 1, 0, -1 };
        public static int[] DiagonalMoves = { -1, 1, 1, -1, -1 };
        public static int[] KnightMoves = { 2, 1, -2, -1, 2, -1, -2, 1, 2 };
        public static int[] BlackPawnMoves = { 1, -1, -1 };
        public static int[] WhitePawnMoves = { -1, 1, 1 };
        public static int[] AdjacentMoves = { 1, -1, 0, 1, 0, -1, -1, 1, 1 };
        public static PieceType[] PromotionPieceTypes = { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight };
        public static PieceType[] StraightPieceTypes = { PieceType.Queen, PieceType.Rook };
        public static PieceType[] DiagonalPieceTypes = { PieceType.Queen, PieceType.Bishop };
        public static PieceType[] KnightPieceTypes = { PieceType.Knight };
        public static PieceType[] PawnTypes = { PieceType.Pawn };

        public static ChessColor FlipTurn( ChessColor color ) => color == ChessColor.White ? ChessColor.Black : ChessColor.White;

        // x and y are co-efficients for what direction the piece is trying to move into
        public static List<Move> GetMovesInDirection( this Board board, int rank, int file, ChessColor color, int x, int y )
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
        public static List<Move> GetRookMoves( this Board board, int rank, int file, ChessColor color )
        {
            List<Move> res = new();
            for ( int i = 0; i < StraightMoves.Length - 1; ++i )
            {
                res.AddRange( GetMovesInDirection( board, rank, file, color, StraightMoves[ i ], StraightMoves[ i + 1 ] ) );
            }
            return res;
        }

        // Get all valid bishop moves from a square on the board
        public static List<Move> GetBishopMoves( this Board board, int rank, int file, ChessColor color )
        {
            List<Move> res = new();
            for ( int i = 0; i < DiagonalMoves.Length - 1; ++i )
            {
                res.AddRange( GetMovesInDirection( board, rank, file, color, DiagonalMoves[ i ], DiagonalMoves[ i + 1 ] ) );
            }
            return res;
        }

        public static List<Move> GetKnightMoves( this Board board, int rank, int file, ChessColor color )
        {
            List<Move> res = new();
            Square from = board.GetSquare( rank, file );

            for ( int i = 0; i < KnightMoves.Length - 1; ++i )
            {
                (int rankTo, int fileTo) = (rank + KnightMoves[ i ], file + KnightMoves[ i + 1 ]);

                if ( board.CanMoveTo( rankTo, fileTo, color ) )
                {
                    res.Add( MoveCreator.CreateMove( board, from, board.GetSquare( rankTo, fileTo ) ) );
                }
            }
            return res;
        }

        public static List<Move> GetKingMoves( this Board board, int rank, int file, ChessColor color )
        {
            List<Move> res = new();
            Square from = board.GetSquare( rank, file );

            for ( int i = 0; i < AdjacentMoves.Length - 1; ++i )
            {
                (int rankTo, int fileTo) = (rank + AdjacentMoves[ i ], file + AdjacentMoves[ i + 1 ]);

                if ( board.CanMoveTo( rankTo, fileTo, color ) )
                {
                    res.Add( MoveCreator.CreateMove( board, from, board.GetSquare( rankTo, fileTo ) ) );
                }
            }
            return res;
        }

        public static int GetDefaultRankForPawn( ChessColor color ) =>
            color == ChessColor.White ? Constants.WHITE_PAWN_STARTING_RANK : Constants.BLACK_PAWN_STARTING_RANK;
        public static int GetEnPassantRankForPawn( ChessColor color ) =>
            color == ChessColor.White ? Constants.WHITE_PAWN_ENPASSANT_RANK : Constants.BLACK_PAWN_ENPASSANT_RANK;
        public static int GetPromotionRankForPawn( ChessColor color ) =>
            color == ChessColor.White ? Constants.WHITE_PAWN_PROMOTION_RANK : Constants.BLACK_PAWN_PROMOTION_RANK;

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

        public static (CRank rank, CFile file) ReadChessNotation( this string s )
        {
            if ( s.Length != 2 )
            {
                throw new ArgumentException( "Invalid String Length for Chess Notation" );
            }

            int rankNum = s[ 1 ] - '0';
            int fileNum = 1 + ( char.IsUpper( s[ 0 ] ) ? s[ 0 ] - 'A' : s[ 0 ] - 'a' );

            CRank rank = new( rankNum );
            CFile file = new( fileNum );


            // change these bounds if the board gets bigger
            if ( rankNum < 1 || rankNum > 8 || fileNum < 1 || fileNum > 8 )
            {
                throw new ArgumentException( "Invalid Position for Chess Notation" );
            }

            return (rank, file);
        }

        public static PieceType GetPieceType( char c ) => c switch
        {
            'p' => PieceType.Pawn,
            'N' => PieceType.Knight,
            'B' => PieceType.Bishop,
            'R' => PieceType.Rook,
            'Q' => PieceType.Queen,
            'K' => PieceType.King,
            _ => throw new NotSupportedException( $"Invalid char '{c}'" )
        };

        public static Piece CreatePiece(PieceType type, ChessColor color, GameObject gameObject)
        {
            return type switch
            {
                PieceType.Pawn => new Pawn( gameObject, color ),
                PieceType.Knight => new Knight( gameObject, color ),
                PieceType.Bishop => new Bishop( gameObject, color ),
                PieceType.Rook => new Rook( gameObject, color ),
                PieceType.Queen => new Queen( gameObject, color ),
                PieceType.King => new King( gameObject, color ),
                _ => throw new InvalidOperationException( $"Cannot create a piece of type {type}" ),
            };
        }

        public static ShallowMove ConvertToShallowMove( Move? move )
        {
            if ( move is null )
            {
                return null;
            }

            ShallowBoard.Square from = new( move.From );
            ShallowBoard.Square to = new( move.To );


            if ( move is BasicMove )
            {
                return new ShallowBasicMove( from, to, move.IsCheck, move.IsCheckmate );
            }
            else if ( move is CaptureMove )
            {
                return new ShallowCaptureMove( from, to, move.IsCheck, move.IsCheckmate );
            }
            else if ( move is EnPassant enPassant )
            {
                return new ShallowEnPassant( from, to, new( enPassant.CaptureOn ), move.IsCheck, move.IsCheckmate );
            }
            else if(move is Castling castling)
            {
                return new ShallowCastling( from, to, new( castling.RookSquare ), move.IsCheck, move.IsCheckmate );
            }
            else if(move is PromotionBasicMove promotionBasic)
            {
                return new ShallowPromotionBasicMove( from, to, promotionBasic.PromoteTo, move.IsCheck, move.IsCheckmate );
            }
            else if ( move is PromotionCaptureMove promotionCapture )
            {
                return new ShallowPromotionCaptureMove( from, to, promotionCapture.PromoteTo, move.IsCheck, move.IsCheckmate );
            }

            throw new Exception( "no equivalent move" );
        }

        public static ShallowPiece CreateShallowPiece(int rank, int file, ChessColor color, PieceType type)
        {
            return type switch
            {
                PieceType.Pawn => new ShallowPawn( rank, file, color ),
                PieceType.Knight => new ShallowKnight( rank, file, color ),
                PieceType.Bishop => new ShallowBishop( rank, file, color ),
                PieceType.Rook => new ShallowRook( rank, file, color ),
                PieceType.Queen => new ShallowQueen( rank, file, color ),
                PieceType.King => new ShallowKing( rank, file, color ),
                _ => throw new NotImplementedException($"Can't make shallow piece of type {type}"),
            };
        }


        public static int GetPawnStartRank( ChessColor color ) =>
            color == ChessColor.Black ? Constants.BLACK_PAWN_STARTING_RANK : Constants.WHITE_PAWN_STARTING_RANK;
        public static int GetPawnMoveDirection( ChessColor color ) =>
               color == ChessColor.Black ? Constants.BLACK_PAWN_DIRECTION : Constants.WHITE_PAWN_DIRECTION;

        public static string PrintNotation( int rank, int file ) => ( char ) ( 'A' + file - 1 ) + rank.ToString();
    }
}