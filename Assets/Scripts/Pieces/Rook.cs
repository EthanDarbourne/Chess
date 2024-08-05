using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Rook : Piece
    {
        public Rook( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Rook( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {
        }

        public override PieceType Type => PieceType.Rook;

        public override List<Square> GetValidMoves( Board board )
        {
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check forward moves
            List<Square> res = Utilities.GetRookMoves( board, rank, file, Color );

            return res;
        }
    }
}
