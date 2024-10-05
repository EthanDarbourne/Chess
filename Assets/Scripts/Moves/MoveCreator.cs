using Assets.Scripts.Enums;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            var move = new BasicMove( from, to );
            AddChecksToMove( board, move );
            return move;
        }

        public static CaptureMove CreateCaptureMove( Board board, Square from, Square to )
        {
            var move = new CaptureMove( from, to );
            AddChecksToMove( board, move );
            return move;
        }

        public static Castling CreateCastlingMove( Board board, Square from, Square to, Square rookSquare )
        {
            var move = new Castling( from, to, rookSquare );
            AddChecksToMove( board, move );
            return move;
        }

        public static EnPassant CreateEnPassantMove( Board board, Square from, Square to, Square captureOn )
        {
            var move = new EnPassant( from, to, captureOn );
            AddChecksToMove( board, move );
            return move;
        }

        private static void AddChecksToMove(Board board, Move move)
        {
            // need to simulate move on board to look for checks, need to pass move object in

            ShallowBoard shallowBoard = board.GetShallowBoard();

            move.ExecuteShallowMove( shallowBoard );

            ChessColor kingColor = move.From.Piece.Color == ChessColor.Black ? ChessColor.White : ChessColor.Black;
            (bool isCheck, bool isCheckmate) = shallowBoard.LookForChecksOnKing( kingColor );

            move.SetChecks( isCheck, isCheckmate );
            
        }

        private static void CheckIfValidMove(Board board, Move move)
        {
            // need to simulate move on board to look for checks, need to pass move object in

            ShallowBoard shallowBoard = board.GetShallowBoard();

            move.ExecuteShallowMove( shallowBoard );

            // analyze color opposite to the one that is moving
            ChessColor colorToCheck = move.From.Piece.Color;


        }
    }
}
