namespace Assets.Scripts.Enums
{
    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King,
        Empty
    }

    public static class PieceTypeExtensions
    {
        public static char GetChar( this PieceType type )
        {
            return type switch {
            PieceType.Pawn => 'p',
            PieceType.Knight => 'N',
            PieceType.Bishop => 'B',
            PieceType.Rook => 'R',
            PieceType.Queen => 'Q',
            PieceType.King => 'K',
            _ => ' ',
            };
        }
    }
}
