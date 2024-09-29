using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Knight : Piece
    {
        public Knight( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Knight( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {
        }

        protected override List<Move> GetPotentialMoves( Board board )
        {
            // check all valid moves for a knight
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check knight moves
            List<Move> res = Utilities.GetKnightMoves( board, rank, file, Color );

            return res;
        }

        public override PieceType Type => PieceType.Knight;
    }
}
