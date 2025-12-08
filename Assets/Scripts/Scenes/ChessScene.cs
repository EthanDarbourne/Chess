using Assets.GameObjects;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System.Collections.Generic;
using UnityEngine;

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
            BoardInteractions.CheckForPlayerInput( Board, MainCamera, _promotionSelector);

            //if ( LastMove != _board.MoveCount )
            //{
            //    LastMove = _board.MoveCount;
            //    try
            //    {
            //        (bool isCheck, bool isCheckmate) = _board.LookForChecks( _board.Turn );
            //        CustomLogger.LogDebug( $"Success with {isCheck} and {isCheckmate}" );
            //    }
            //    catch ( Exception ex )
            //    {
            //        CustomLogger.LogDebug( $"Failed with {ex.Message}" );
            //    }
            //}

        }

        private void MapPiecesToBoard()
        {
            Board = BoardInteractions.CreateBoard( PieceManager, _promotionSelector, this );

            Board.SetupForGameStart();

            if (DoMovesAtStartOfGame)
            {
                MovesAtStartOfGame.ForEach(Board.MovePiece);
            }

        }
    }
}
