﻿using Assets.Scripts.Parts;
using System;

namespace Assets.Scripts.Moves
{
    public abstract class Move
    {
        protected Square _from;
        protected Square _to;

        protected bool _isCheck;
        protected bool _isCheckmate;

        private bool _isExecuted = false;

        protected Move( Square from, Square to, bool isCheck = false, bool isCheckmate = false )
        {
            _from = from;
            _to = to;
            _isCheck = isCheck;
            _isCheckmate = isCheckmate;
        }

        protected abstract void DoExecuteMove( Board board );

        protected abstract void DoUndoMove( Board board );


        public void ExecuteMove(Board board)
        {
            if( _isExecuted )
            {
                throw new Exception( "Already executed" );
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

        public Square From => _from;
        public Square To => _to;
    }
}