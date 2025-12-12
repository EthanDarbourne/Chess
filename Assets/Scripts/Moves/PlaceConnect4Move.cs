using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;

namespace Assets.Scripts.Moves
{
    public class PlaceConnect4Move : Move
    {
        private Piece _connect4Piece;
        public PlaceConnect4Move(Piece piece, Square from, Square to, string boardHash, bool isCheck = false, bool isCheckmate = false) : base(from, to, boardHash, isCheck, isCheckmate)
        {
            if (piece is not Connect4)
            {
                throw new System.Exception("This is the wrong piece type");
            }
            _connect4Piece = piece;
        }

        public override void ExecuteShallowMove(ShallowBoard board)
        {
            throw new System.NotImplementedException();
        }

        protected override void DoExecuteMove(Board board)
        {
            int file = To.File.Num;
            int targetRank = board.Height;
            while (targetRank > 1 && board.GetSquare(targetRank - 1, file).Piece is null)
            {
                targetRank -= 1;
            }
            board.SetPiece(targetRank, file, _connect4Piece);
        }

        protected override void DoUndoMove(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}
