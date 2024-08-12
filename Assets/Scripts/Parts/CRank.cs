namespace Assets.Scripts.Parts
{
    public class CRank
    {
        private int _rank;

        public CRank( CRank rank )
        {
            _rank = rank.Num;
        }

        public CRank( int rank )
        {
            _rank = rank;
        }

        public int Num => _rank;

        public int AddNum( int change )
        {
            _rank += change;
            return _rank;
        }

        public static int operator -( CRank a, CRank b ) => a.Num - b.Num;

    }

    public static class CRankExtensions
    {
        public static bool Equals( this CRank a, int b ) => a.Num == b;
    }
}