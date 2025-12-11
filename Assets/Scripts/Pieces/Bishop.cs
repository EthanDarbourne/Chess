using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.ShallowCopy;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public class Bishop : Piece
    {
        public Bishop( GameObject gamePiece, CRank rank, CFile file, ChessColor color ) : base( gamePiece, rank, file, color )
        {
        }

        public Bishop( GameObject gamePiece, ChessColor color ) : base( gamePiece, color )
        {
        }

        public override List<Move> GetPotentialMoves( Board board )
        {
            // check all valid moves for a bishop
            int rank = _location.Rank.Num;
            int file = _location.File.Num;

            // check bishop moves
            List<Move> res = board.GetBishopMoves( rank, file, Color );

            return res;
        }

        public override ShallowPiece CreateShallowPiece() => 
            new ShallowBishop( _location.Rank.Num, _location.File.Num, Color);

        public override PieceType Type => PieceType.Bishop;
    }
}
