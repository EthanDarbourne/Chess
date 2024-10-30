using Assets.Scripts.Parts;

namespace Assets.Scripts.Moves
{
    public abstract class ShallowMove : IExecuteShallowMove
    {
        protected ShallowBoard.Square _from;
        protected ShallowBoard.Square _to;

        protected bool _isCheck;
        protected bool _isCheckmate;

        protected ShallowMove( ShallowBoard.Square from, ShallowBoard.Square to, bool isCheck = false, bool isCheckmate = false )
        {
            _from = from;
            _to = to;
            _isCheck = isCheck;
            _isCheckmate = isCheckmate;
        }
        public ShallowBoard.Square From => _from;
        public ShallowBoard.Square To => _to;

        public abstract void ExecuteShallowMove( ShallowBoard board );

        public void SetChecks( bool isCheck, bool isCheckmate )
        {
            _isCheck = isCheck;
            _isCheckmate = isCheckmate;
        }
    }
}
