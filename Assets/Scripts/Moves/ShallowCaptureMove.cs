using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Moves
{
    public class ShallowCaptureMove : Move
    {
        private new readonly ShallowBoard.Square From;
        private new readonly ShallowBoard.Square To;


        public ShallowCaptureMove( ShallowBoard.Square from, ShallowBoard.Square to, bool isCheck = false, bool isCheckmate = false )
            : base( null, null, isCheck, isCheckmate )
        {
            From = from;
            To = to;
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int rank1, int file1) = (From.Rank, From.File);
            (int rank2, int file2) = (To.Rank, To.File);
            board.CaptureSquare( rank1, file1, rank2, file2 );
            board.OnMoveExecuted();
        }

        protected override void DoExecuteMove( Board board )
        {
            throw new NotSupportedException( "Not supported on Shallow Moves" );
        }

        protected override void DoUndoMove( Board board )
        {
            throw new NotSupportedException( "Not supported on Shallow Moves" );
        }
    }
}
