using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.ShallowCopy;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Connect4 : Piece
    {
        public Connect4(GameObject gamePiece, CRank rank, CFile file, ChessColor color) : base(gamePiece, rank, file, color)
        {
        }

        public Connect4(GameObject gamePiece, ChessColor color) : base(gamePiece, color)
        {
        }

        public override List<Move> GetPotentialMoves(Board board)
        {
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // only move is to go down via gravity
            List<Move> moves = board.GetMovesInDirection(rank, file, ChessColor.Black, 0, -1);

            if(moves.Count == 0)
            {
                return moves;
            }
            int lowestRank = rank;
            Move bestMove = moves[0];
            foreach (Move move in moves)
            {
                if (move is CaptureMove)
                {
                    continue;
                }
                if(move.To.Rank.Num < lowestRank)
                {
                    bestMove = move;
                }
            }
            return new List<Move> { bestMove };
        }

        public override ShallowPiece CreateShallowPiece() =>
            throw new NotImplementedException();

        public override PieceType Type => PieceType.Connect4;
    }
}
