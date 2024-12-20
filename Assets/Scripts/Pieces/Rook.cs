﻿using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.ShallowCopy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Rook : Piece
    {
        private bool _hasntMoved = true;

        public Rook( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Rook( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {
        }

        public override PieceType Type => PieceType.Rook;

        public bool HasntMoved => _hasntMoved;

        public override void Move( int rankChange, int fileChange )
        {
            _hasntMoved = false;
            base.Move( rankChange, fileChange );
        }

        protected override List<Move> GetPotentialMoves( Board board )
        {
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check forward moves
            List<Move> res = board.GetRookMoves( rank, file, Color );

            return res;
        }

        public override ShallowPiece CreateShallowPiece() =>
            new ShallowRook( _location.Rank.Num, _location.File.Num, Color );
    }
}
