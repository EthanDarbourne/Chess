using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.GameObjects;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using System;
using UnityEngine;

namespace Assets.Scripts.Scenes
{
    public static class BoardInteractions
    {
        public static T CreateBoard<T>(PieceManager pieceManager, PromotionSelector promotionSelector, MonoBehaviour parent) where T : Board
        {
            GameObject boardObj = pieceManager.CreateBoard(new Vector3(-4f, 0, 4f));

            GameObject whiteGraveyardObj = pieceManager.CreatePieceGraveyard(new Vector3(-8, 0, 4));
            GameObject blackGraveyardObj = pieceManager.CreatePieceGraveyard(new Vector3(6, 0, 4));

            PieceGraveyard whitePieceGraveyard = new(whiteGraveyardObj, ChessColor.White);
            PieceGraveyard blackPieceGraveyard = new(blackGraveyardObj, ChessColor.Black);

            PieceGraveyard[] pieceGraveyards = new PieceGraveyard[]
            {
                whitePieceGraveyard,
                blackPieceGraveyard
            };

            boardObj.AddComponent<BoxCollider>();

            HighlightSquare defaultHighlightSquare = new(Creator.CreateHighlightSquare(boardObj), CommonVectors.FirstLayerHeightOffset);
            HighlightSquare inCheckHighlightSquare = new(Creator.CreateInCheckHighlightSquare(boardObj), CommonVectors.SecondLayerHeightOffset);

            return (T)Activator.CreateInstance(typeof(T), Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT, pieceManager, pieceGraveyards, boardObj, defaultHighlightSquare, inCheckHighlightSquare, promotionSelector, parent);
        }

        public static void CheckForPlayerInput(Board board, CameraManager cameraManager, PromotionSelector promotionSelector)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = cameraManager.EnabledCamera.ScreenPointToRay(mousePos);
                ray.direction = ray.direction * 100;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000000f))
                {
                    string name = raycastHit.transform.name;
                    if (promotionSelector.IsSelectorOpen)
                    {
                        if (name.StartsWith("Promotion"))
                        {
                            promotionSelector.Trigger(raycastHit.point.x, raycastHit.point.z);
                        }
                        else
                        {
                            promotionSelector.Hide();
                        }
                    }
                    else if (raycastHit.transform is not null) // .name.StartsWith(_pieceManager.Board.name )
                    {
                        int file = (int)(4 + Math.Ceiling(raycastHit.point.x));
                        int rank = (int)(4 + Math.Ceiling(raycastHit.point.z));

                        board.SelectLocation(rank, file);
                    }
                    else
                    {
                        CustomLogger.LogDebug("Deselecting piece");
                        board.DeselectPiece();
                    }

                }
                else
                {
                    if (promotionSelector.IsSelectorOpen)
                    {
                        promotionSelector.Hide();
                    }
                    board.DeselectPiece();
                }
            }
            CheckForKeyPresses(board);
        }

        private static void CheckForKeyPresses(Board board)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // game to end
                board.ExecuteAllMoves();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // game to start
                board.UndoAllMoves();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // back one move
                board.UndoOneMove();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // forward one move
                board.ExecuteOneMove();
            }
        }
    }
}
