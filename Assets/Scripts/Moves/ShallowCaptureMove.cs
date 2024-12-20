﻿using Assets.Scripts.Parts;

namespace Assets.Scripts.Moves
{
    public class ShallowCaptureMove : ShallowMove
    {
        public ShallowCaptureMove( ShallowBoard.Square from, ShallowBoard.Square to, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int rank1, int file1) = (From.Rank, From.File);
            (int rank2, int file2) = (To.Rank, To.File);
            board.CaptureSquare( rank1, file1, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
