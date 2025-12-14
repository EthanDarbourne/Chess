using Assets.Scripts.Parts;
using System;

namespace Assets.Scripts.Moves
{
    public abstract class Move : IExecuteShallowMove
    {
        protected Square _from;
        protected Square _to;

        protected MoveInfo _moveInfo;

        //protected bool _isCheck;
        //protected bool _isCheckmate;

        private bool _isExecuted = false;

        private string _moveNotation = string.Empty;

        private string _boardHash;

        protected Move( Square from, Square to, string boardHash, bool isCheck = false, bool isCheckmate = false )
        {
            _from = from;
            _to = to;
            _boardHash = boardHash;
            bool isCaptureMove = this is CaptureMove || this is EnPassant;
            _moveInfo = new(from.Piece?.Type == Enums.PieceType.Pawn, isCaptureMove);
            //_isCheck = isCheck;
            //_isCheckmate = isCheckmate;
        }

        public Square From => _from;
        public Square To => _to;
        //public bool IsCheck => _isCheck;
        //public bool IsCheckmate => _isCheckmate;
        public string MoveNotation => _moveNotation;

        public bool IsPawnMove => _moveInfo.IsPawnMove;
        public bool IsCaptureMove => _moveInfo.IsCaptureMove;

        public int GetLength()
        {
            return GetFileLength() + GetRankLength();
        }

        public int GetFileLength()
        {
            return Math.Abs( To.File.Num - From.File.Num );
        }

        public int GetRankLength()
        {
            return Math.Abs( To.Rank.Num - From.Rank.Num );
        }

        public void SetNotation(string notation)
        {
            _moveNotation = notation;
        }

        protected abstract void DoExecuteMove( Board board );

        protected abstract void DoUndoMove( Board board );

        public abstract void ExecuteShallowMove( ShallowBoard board );

        public void SetChecks(bool isCheck, bool isCheckmate)
        {
            //_isCheck = isCheck;
            //_isCheckmate = isCheckmate;
        }

        public void ExecuteMove( Board board )
        {
            if( _isExecuted )
            {
                throw new Exception( "Already executed" );
            }
            if(_boardHash != board.GetBoardHash())
            {
                throw new Exception("Incorrect board state for this move");
            }
            DoExecuteMove( board );
            _isExecuted = true;
        }

        public void UndoMove(Board board)
        {
            if ( !_isExecuted )
            {
                throw new Exception( "Hasn't been executed" );
            }
            DoUndoMove( board );
            _isExecuted = false;
        }

        public void EnableMoveToHighlight()
        {
            To.EnableMoveToHighlight();
        }
    }
}
