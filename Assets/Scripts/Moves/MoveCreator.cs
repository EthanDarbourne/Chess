using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Moves
{
    public static class MoveCreator
    {

        public static Move CreateMove(Board board, Square from, Square to)
        {
            if(to.IsCapturable(from.Piece.Color))
            {
                return CreateCaptureMove( board, from, to );
            }
            return CreateBasicMove( board, from, to );
        }

        public static BasicMove CreateBasicMove(Board board, Square from, Square to)
        {
            (bool isCheck, bool isCheckmate) = LookForChecks( board, from, to );

            return new BasicMove( from, to, isCheck, isCheckmate );
        }

        public static CaptureMove CreateCaptureMove( Board board, Square from, Square to )
        {
            (bool isCheck, bool isCheckmate) = LookForChecks( board, from, to );

            return new CaptureMove( from, to, isCheck, isCheckmate );
        }

        public static Castling CreateCastlingMove( Board board, Square from, Square to, Square rookSquare )
        {
            (bool isCheck, bool isCheckmate) = LookForChecks( board, from, to );

            return new Castling( from, to, rookSquare, isCheck, isCheckmate );
        }

        public static EnPassant CreateEnPassantMove( Board board, Square from, Square to, Square captureOn )
        {
            (bool isCheck, bool isCheckmate) = LookForChecks( board, from, to );

            return new EnPassant( from, to, captureOn, isCheck, isCheckmate );
        }

        private static (bool isCheck, bool isCheckmate) LookForChecks(Board board, Square from, Square to)
        {
            // need to simulate move on board to look for checks, need to pass move object in

            return (false, false);
        }
    }
}
