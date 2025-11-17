using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainScript2 : MonoBehaviour
    {
        public Camera MainCamera;

        private Board Board;

        public PieceManager PieceManager;

        public PromotionSelector PromotionSelector;

        public PieceGraveyard PieceGraveyard;

        // Start is called before the first frame update
        void Start()
        {
            CustomLogger.CurrentLogLevel = LogLevel.Debug;

            MainCamera.enabled = true;

            PromotionSelector = new PromotionSelector(PieceManager.CreatePromotionSelector());

            MapPiecesToBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = MainCamera.ScreenPointToRay( mousePos );
                //CustomLogger.LogDebug( $"Clicked on {mousePos.x}, {mousePos.y}" );


                ray.direction = ray.direction * 100;

                if ( Physics.Raycast( ray, out RaycastHit raycastHit, 1000000f ) )
                {
                    CustomLogger.LogDebug( $"Hit {raycastHit.transform.name} {PieceManager.Board.name}" );
                    if (PromotionSelector.IsSelectorOpen)
                    {
                        CustomLogger.LogDebug( $"Hit promotion selector at {raycastHit.point}" );
                        PromotionSelector.Trigger( raycastHit.point.x, raycastHit.point.z );
                    }
                    else if ( raycastHit.transform is not null ) // .name.StartsWith(_pieceManager.Board.name )
                    {

                        int file = ( int ) ( 4 + Math.Ceiling( raycastHit.point.x ) );
                        int rank = ( int ) ( 4 + Math.Ceiling( raycastHit.point.z) );

                        CustomLogger.LogDebug( $"Clicked on BOARD {file}, {rank}" );

                        Board.SelectLocation( rank, file );
                    }
                    else
                    {
                        Board.DeselectPiece();
                    }
                    
                }
            }
            CheckForKeyPresses();

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

        private GameObject CreateHighlightSquare( GameObject parent )
        {
            GameObject plane = Creator.CreatePlane();
            plane.transform.SetParent(parent.transform, false);
            return plane;
        }

        private void MapPiecesToBoard()
        {
            GameObject boardObj = PieceManager.CreateBoard(new Vector3(-4f, 0, 4f));

            GameObject pieceGraveyardObj = PieceManager.CreatePieceGraveyard(new Vector3(6, 0, 4));

            PieceGraveyard = new PieceGraveyard(pieceGraveyardObj);

            //GameObject pawn = PieceManager.GeneratePiece(PieceType.Pawn, ChessColor.White, boardObj);
            //Piece pawnPiece = new Pawn(pawn, ChessColor.White);

            //PieceGraveyard.SendPieceToGraveyard(pawnPiece);

            boardObj.AddComponent<BoxCollider>();

            HighlightSquare highlightSquare = new( CreateHighlightSquare( boardObj ) );

            Board = new( Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT, PieceManager, boardObj, highlightSquare, PromotionSelector, this );

            Board.SetupForGameStart();
        }

        private void CheckForKeyPresses()
        {
            if ( Input.GetKeyDown( KeyCode.DownArrow ) )
            {
                // game to end
                Board.ExecuteAllMoves();
            }
            else if ( Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                // game to start
                Board.UndoAllMoves();
            }
            else if ( Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                // back one move
                Board.UndoOneMove();
            }
            else if ( Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                // forward one move
                Board.ExecuteOneMove();
            }
        }
    }
}
