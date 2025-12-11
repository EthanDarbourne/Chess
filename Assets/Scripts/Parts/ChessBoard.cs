using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class ChessBoard : Board
    {
        public ChessBoard(int width, int height, PieceManager pieceManager, PieceGraveyard[] pieceGraveyards, GameObject boardObject, HighlightSquare defaultHighlightSquare, HighlightSquare inCheckHighlightSquare, PromotionSelector selector, MonoBehaviour monoBehaviour) : base(width, height, pieceManager, pieceGraveyards, boardObject, defaultHighlightSquare, inCheckHighlightSquare, selector, monoBehaviour)
        {
        }

        public override bool IsValidPositionAfterMove(Move move)
        {
            ShallowBoard shallowBoard = GetShallowBoard();
            move.ExecuteShallowMove(shallowBoard);
            return shallowBoard.IsValidPosition();
        }

        protected override void DoPlayerInput(int rank, int file)
        {
            if (_movePieceCoroutine is not null) // todo: could cancel here
            {
                CustomLogger.LogDebug("A movement is already in progress, ignoring input");
                return;
            }
            if (_selectedPiece is not null)
            {
                CustomLogger.LogDebug("We have a selected piece, trying to move it");
                _movePieceCoroutine = _monoBehaviour.StartCoroutine(TryToMoveSelectedPieceToLocation(rank, file));
            }
            else
            {
                CustomLogger.LogDebug("We don't have a piece selected");
                SelectPiece(rank, file);
            }
        }

        protected override void SetupBoard()
        {
            MovePiecesToStartSquares(Constants.DEFAULT_BACKLINE);
        }
        protected void MovePiecesToStartSquares(List<PieceType> backline)
        {
            _whitePieces.AddRange(GeneratePieces(backline, ChessColor.White, Constants.WHITE_BACKLINE_RANK));
            _blackPieces.AddRange(GeneratePieces(backline, ChessColor.Black, Constants.BLACK_BACKLINE_RANK));

            List<PieceType> pawns = Enumerable.Repeat(PieceType.Pawn, 8).ToList();
            _whitePieces.AddRange(GeneratePieces(pawns, ChessColor.White, Constants.WHITE_PAWN_STARTING_RANK));
            _blackPieces.AddRange(GeneratePieces(pawns, ChessColor.Black, Constants.BLACK_PAWN_STARTING_RANK));
        }

        protected override void OnMoveExecuted()
        {
            // after the turn, see if opponent king is in check
            (bool isCheck, bool isCheckmate) = LookForChecks(_turn);
            HighlightKing(isCheck);

            if (isCheckmate)
            {

                if (_turn == ChessColor.Black)
                {
                    GameOver(GameState.CheckmateWhite);
                    CustomLogger.LogInfo("Checkmate! White wins!");
                }
                else if (_turn == ChessColor.White)
                {
                    GameOver(GameState.CheckmateBlack);
                    CustomLogger.LogInfo("Checkmate! Black wins!");
                }
            }
        }
    }
}
