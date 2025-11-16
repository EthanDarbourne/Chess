using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Parts
{
    
    public class BoardState
    {
        protected struct CheckInfo
        {
            public bool IsInCheck { get; set; }
            public bool IsCheckmate { get; set; }
            public ChessColor KingColor { get; set; }
        }

        private List<Square> _checkingPieces = new();

        private string _boardStateHash = string.Empty;

        public BoardState()
        {

        }

        public void InitializeCheckState(Board board)
        {
            _boardStateHash = board.GetBoardHash();

            //board.LookForChecks()

        }
    }
}
