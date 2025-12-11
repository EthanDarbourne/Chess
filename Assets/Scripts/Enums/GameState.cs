namespace Assets.Scripts.Enums
{
    public enum GameState
    {
        HasntStarted,
        InProgress,
        CheckmateWhite,
        CheckmateBlack,
        Stalemate,
        DrawByAgreement,
        DrawByInsufficientMaterial,
        DrawByFiftyMoveRule,
        DrawByThreefoldRepetition,

        // Connect4 Game States
        WhiteWin,
        BlackWin
    }
}
