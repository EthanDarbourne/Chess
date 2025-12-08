using Assets.GameObjects;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Scenes
{
    public class ChessScene : MonoBehaviour
    {
        public Camera MainCamera;

        private Board Board;

        public PieceManager PieceManager;

        private PromotionSelector _promotionSelector;

        public bool DoMovesAtStartOfGame = false;
        public List<string> MovesAtStartOfGame = new();

        private bool _playingGame = false;

        // Start is called before the first frame update
        void Start()
        {
            CustomLogger.CurrentLogLevel = LogLevel.Debug;

            MainCamera.enabled = true;

            _promotionSelector = new PromotionSelector(PieceManager.CreatePromotionSelector());

            MapPiecesToBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if (Board.IsGameInProgress)
            {
                BoardInteractions.CheckForPlayerInput( Board, MainCamera, _promotionSelector);
            }

            if(!Board.IsGameInProgress && _playingGame)
            {
                _playingGame = false;
                CustomLogger.LogInfo("Chess Game Over");

                GameState gameState = Board.GetGameState();

                if (gameState == GameState.CheckmateWhite)
                {
                    CustomLogger.LogInfo("Checkmate! White wins!");
                }
                else if (gameState == GameState.CheckmateBlack)
                {
                    CustomLogger.LogInfo("Checkmate! Black wins!");
                }
                else
                {
                    CustomLogger.LogInfo("Stalemate!");
                }
            }

        }

        private void MapPiecesToBoard()
        {
            Board = BoardInteractions.CreateBoard( PieceManager, _promotionSelector, this );

            Board.SetupForGameStart();

            Board.StartGame();

            if (DoMovesAtStartOfGame)
            {
                MovesAtStartOfGame.ForEach(Board.MovePiece);
            }

            _playingGame = true;
            CustomLogger.LogInfo("Starting basic Chess game");
        }
    }
}
