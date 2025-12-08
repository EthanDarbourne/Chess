namespace Assets.Scripts.Enums
{
    public enum GameState
    {
        InProgress,
        CheckmateWhite,
        CheckmateBlack,
        Stalemate,
        DrawByAgreement,
        DrawByInsufficientMaterial,
        DrawByFiftyMoveRule,
        DrawByThreefoldRepetition
    }
}
