namespace Assets.Scripts.Moves
{
    public struct MoveInfo
    {
        public bool IsPawnMove;

        public bool IsCaptureMove;

        public MoveInfo(bool isPawnMove, bool isCaptureMove)
        {
            IsPawnMove = isPawnMove;
            IsCaptureMove = isCaptureMove;
        }
    }
}
