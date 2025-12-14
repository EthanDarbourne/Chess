using Assets.GameObjects;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.Scenes
{
    public class ChessVsConnect4Scene : MonoBehaviour
    {
        private ChessVsConnect4Board Board;

        public CameraManager CameraManager;

        public PieceManager PieceManager;

        private PromotionSelector _promotionSelector;

        public bool DoMovesAtStartOfGame = false;
        public List<string> MovesAtStartOfGame = new();

        private bool _playingGame = false;

        // Start is called before the first frame update
        void Start()
        {
            CustomLogger.CurrentLogLevel = LogLevel.Debug;

            _promotionSelector = new PromotionSelector(PieceManager.CreatePromotionSelector());

            MapPiecesToBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if (Board.IsGameInProgress)
            {
                BoardInteractions.CheckForPlayerInput(Board, CameraManager, _promotionSelector);
            }

            if (!Board.IsGameInProgress && _playingGame)
            {
                _playingGame = false;
                CustomLogger.LogInfo("Chess Vs Connect4 Game Over");

                GameState gameState = Board.GameState;

                if (gameState == GameState.WhiteWin)
                {
                    CustomLogger.LogInfo("Yay! White survived");
                }
                else if (gameState == GameState.BlackWin)
                {
                    CustomLogger.LogInfo("Wow, Black got Connect4");
                }
                else
                {
                    CustomLogger.LogInfo("How did this outcome happen? " + gameState);
                }
            }

        }

        private void MapPiecesToBoard()
        {
            Board = BoardInteractions.CreateBoard<ChessVsConnect4Board>(PieceManager, _promotionSelector, this);

            Board.SetupForGameStart();

            Board.StartGame();

            _playingGame = true;
            CustomLogger.LogInfo("Starting Chess vs Connect4 game");
        }
    }
}
