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

        public static Move CreateMove(this Board board, Square from, Square to)
        {
            if(to.IsCapturable(from.Piece.Color))
            {
                return board.CreateCaptureMove( from, to );
            }
            return board.CreateBasicMove( from, to );
        }

        public static BasicMove CreateBasicMove(this Board board, Square from, Square to)
        {
            var move = new BasicMove( from, to );
            board.AddChecksToMove( move );
            return move;
        }

        public static CaptureMove CreateCaptureMove( this Board board, Square from, Square to )
        {
            var move = new CaptureMove( from, to );
            board.AddChecksToMove( move );
            return move;
        }

        public static Castling CreateCastlingMove( this Board board, Square from, Square to, Square rookSquare )
        {
            var move = new Castling( from, to, rookSquare );
            board.AddChecksToMove( move );
            return move;
        }

        public static EnPassant CreateEnPassantMove( this Board board, Square from, Square to, Square captureOn )
        {
            var move = new EnPassant( from, to, captureOn );
            board.AddChecksToMove( move );
            return move;
        }

        public static ShallowBasicMove CreateShallowBasicMove( this ShallowBoard board, ShallowBoard.Square from, ShallowBoard.Square to )
        {
            var move = new ShallowBasicMove( from, to );
            //board.AddChecksToMove( move );
            return move;
        }

        public static ShallowCaptureMove CreateShallowCaptureMove( this ShallowBoard board, ShallowBoard.Square from, ShallowBoard.Square to )
        {
            var move = new ShallowCaptureMove( from, to );
            //board.AddChecksToMove( move );
            return move;
        }

        private static void AddChecksToMove(this Board board, Move move)
        {
            // need to simulate move on board to look for checks, need to pass move object in

            ShallowBoard shallowBoard = board.GetShallowBoard();

            shallowBoard.AddChecksToMove( move );
        }

        private static void AddChecksToMove(this ShallowBoard board, Move move)
        {
            move.ExecuteShallowMove( board );

            ChessColor kingColor = move.From.Piece.Color == ChessColor.Black ? ChessColor.White : ChessColor.Black;
            (bool isCheck, bool isCheckmate) = board.LookForChecksOnKing( kingColor );

            move.SetChecks( isCheck, isCheckmate );
        }

        private static void CheckIfValidMove(this Board board, Move move)
        {
            // need to simulate move on board to look for checks, need to pass move object in

            ShallowBoard shallowBoard = board.GetShallowBoard();

            move.ExecuteShallowMove( shallowBoard );

            // analyze color opposite to the one that is moving
            ChessColor colorToCheck = move.From.Piece.Color;


        }
    }
}
