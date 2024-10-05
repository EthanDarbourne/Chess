using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using UnityEngine;

namespace Assets.Scripts.Moves
{
    public class BasicMove : Move
    {
        public BasicMove(Square from, Square to, bool isCheck = false, bool isCheckmate = false )
            : base(from, to, isCheck, isCheckmate)
        {

        }

        protected override void DoExecuteMove( Board board )
        {
            Piece movingPiece = From.RemovePiece();
            To.MovePieceTo( movingPiece );
        }

        protected override void DoUndoMove( Board board )
        {
            Piece movingPiece = To.RemovePiece();
            From.MovePieceTo( movingPiece );
        }

        public override void ExecuteShallowMove( ShallowBoard board )
        {
            (int rank1, int file1) = (From.Rank.Num, From.File.Num);
            (int rank2, int file2) = (To.Rank.Num, To.File.Num);
            Debug.Log( $"Moving to {rank2}, {file2}" );
            board.SwapSquares( rank1, file1, rank2, file2 );
            board.OnMoveExecuted();
        }
    }
}
