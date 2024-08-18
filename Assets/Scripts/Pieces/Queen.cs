﻿using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Queen : Piece
    {

        public Queen( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Queen( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {

        }

        public override PieceType Type => PieceType.Queen;

        public override List<Move> GetValidMoves( Board board )
        {
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            List<Move> res = Utilities.GetRookMoves( board, rank, file, Color );
            res.AddRange( Utilities.GetBishopMoves( board, rank, file, Color ) );

            return res;
        }
    }
}