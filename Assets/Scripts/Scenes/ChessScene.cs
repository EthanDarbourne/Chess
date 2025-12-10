using Assets.GameObjects;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.Scenes
{
    public class ChessScene : MonoBehaviour
    {
        public Camera WhiteCamera;

        public Camera SideCamera;

        public Camera BlackCamera;

        private CameraManager _cameraManager = new();

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

            _cameraManager.RegisterCamera(WhiteCamera);
            _cameraManager.RegisterCamera(SideCamera); // 12,6,0, 30, 270
            _cameraManager.RegisterCamera(BlackCamera); // 0,9,5 120,0,180

            _cameraManager.EnableCamera(WhiteCamera);

            _promotionSelector = new PromotionSelector(PieceManager.CreatePromotionSelector());

            MapPiecesToBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if (Board.IsGameInProgress)
            {
                BoardInteractions.CheckForPlayerInput( Board, _cameraManager, _promotionSelector);
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
