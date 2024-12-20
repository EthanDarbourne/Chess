﻿using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Moves
{
    public class Castling : Move
    {
        private Square _rookSquare;
        // from is king square, to is rook square (todo: from is king, to is clickable square)
        public Castling( Square from, Square to, Square rookSquare, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
            _rookSquare = rookSquare;
        }

        public Square RookSquare => _rookSquare;

        protected override void DoExecuteMove( Board board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();

            Piece king = From.RemovePiece();
            Piece rook = _rookSquare.RemovePiece();

            Square rookNewSquare = board.GetSquare( To.Rank.Num, rookNewFile );
            Square kingNewSquare = board.GetSquare( To.Rank.Num, kingNewFile );

            rookNewSquare.MovePieceTo( rook );
            kingNewSquare.MovePieceTo( king );
        }

        protected override void DoUndoMove( Board board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();
            Square rookNewSquare = board.GetSquare( To.Rank.Num, rookNewFile );
            Square kingNewSquare = board.GetSquare( To.Rank.Num, kingNewFile );
            Piece king = kingNewSquare.RemovePiece();
            Piece rook = rookNewSquare.RemovePiece();
            From.MovePieceTo( king );
            _rookSquare.MovePieceTo( rook );
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int kingNewFile, int rookNewFile) = GetNewKingAndRookFiles();

            int rank = To.Rank.Num;
            board.SwapSquares( rank, From.File.Num, rank, kingNewFile );
            board.SwapSquares( rank, _rookSquare.File.Num, rank, rookNewFile );
            board.OnMoveExecuted();
        }

        private (int KingFile, int RookFile) GetNewKingAndRookFiles()
        {
            int kingFile = From.File.Num;
            int rookFile = _rookSquare.File.Num;
            int dir = rookFile < kingFile ? -1 : 1;
            int kingNewFile = kingFile + dir * 2;
            int rookNewFile = kingNewFile - dir;
            return (kingNewFile, rookNewFile);
        }
    }
}
