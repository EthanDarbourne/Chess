using Assets.Scripts.Enums;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Pawn : Piece
    {

        private bool _hasMoved = false;

        public Pawn( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Pawn( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {

        }

        public override PieceType Type => PieceType.Pawn;

        public override List<Square> GetValidMoves( Board board )
        {
            
            List<Square> res = new();

            // check all valid moves for a pawn
            int file = _location.File.Num;
            int rank = _location.Rank.Num;

            int oneStep = Color == ChessColor.White ? 1 : -1;
            int twoStep = oneStep * 2;

            // check forward moves
            if ( board.CanMoveTo( rank + oneStep, file, Color ) )
            {
                res.Add( board.GetSquare( rank + oneStep, file ) );

                if ( !_hasMoved && board.CanMoveTo( rank + twoStep, file, Color ) )
                {
                    res.Add( board.GetSquare( rank + twoStep, file ) );
                }
            }

            // check diagonal capture
            if ( board.CanCapture( rank + oneStep, file - 1, Color ) )
            { // make can capture
                res.Add( board.GetSquare( rank + oneStep, file - 1 ) );
            }

            if ( board.CanCapture( rank + oneStep, file + 1, Color ) )
            {
                res.Add( board.GetSquare( rank + oneStep, file + 1 ) );
            }
            return res;
        }

        public override void Move( int rankChange, int fileChange )
        {
            _hasMoved = true;
            base.Move( rankChange, fileChange );
        }

    }
}
