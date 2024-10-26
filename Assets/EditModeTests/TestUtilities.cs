//using Assets.Scripts.Enums;
//using Assets.Scripts.Misc;
//using Assets.Scripts.Parts;
//using Assets.Scripts.Pieces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.EditModeTests
//{
//    public static class TestUtilities
//    {

//        private static readonly List<string> _pieces = new()
//        {
//            "BishopC1",
//            "BishopC8",
//            "BishopF1",
//            "BishopF8",
//            "KingE1",
//            "KingE8",
//            "KnightB1",
//            "KnightB8",
//            "KnightG1",
//            "KnightG8",
//            "PawnA2",
//            "PawnA7",
//            "PawnB2",
//            "PawnB7",
//            "PawnC2",
//            "PawnC7",
//            "PawnD2",
//            "PawnD7",
//            "PawnE2",
//            "PawnE7",
//            "PawnF2",
//            "PawnF7",
//            "PawnG2",
//            "PawnG7",
//            "PawnH2",
//            "PawnH7",
//            "QueenD1",
//            "QueenD8",
//            "RookA1",
//            "RookA8",
//            "RookH1",
//            "RookH8"
//        };

//        public static Board GetSetupBoard()
//        {
//            var board = new Board( 8, 8, new(), new( new() ) );

//            foreach ( var name in _pieces )
//            {
//                string type = name[ ..^2 ];
//                string position = name[ ^2.. ];
//                ChessColor color = position[ 1 ] == '1' || position[ 1 ] == '2' ? ChessColor.White : ChessColor.Black;
//                var gamePiece = new GameObject();
//                Piece piece = type switch
//                {
//                    "Pawn" => new Pawn( gamePiece, color ),
//                    "Knight" => new Knight( gamePiece, color ),
//                    "Bishop" => new Bishop( gamePiece, color ),
//                    "Rook" => new Rook( gamePiece, color ),
//                    "Queen" => new Queen( gamePiece, color ),
//                    "King" => new King( gamePiece, color ),
//                    _ => throw new InvalidOperationException( $"Cannot create a piece of type {type}" ),
//                };
//                (CRank rank, CFile file) = position.ReadChessNotation();
//                board.SetPiece( rank, file, piece );
//            }
//            board.SetupForGameStart();
//            return board;
//        }
//    }
//}
