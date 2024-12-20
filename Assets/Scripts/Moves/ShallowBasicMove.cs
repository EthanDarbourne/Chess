﻿using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Moves
{
    public class ShallowBasicMove : ShallowMove
    {
        public ShallowBasicMove( ShallowBoard.Square from, ShallowBoard.Square to, bool isCheck = false, bool isCheckmate = false )
            : base( from, to, isCheck, isCheckmate )
        {
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int rank1, int file1) = (From.Rank, From.File);
            (int rank2, int file2) = (To.Rank, To.File);
            board.SwapSquares( rank1, file1, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
