using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Moves;
using Assets.Scripts.Pieces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class ChessBoard : Board
    {

        private Dictionary<string, int> _positionCounts = new();
        public int _movesWithoutPawnMoveOrCapture = 0;

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
            _whitePieces.Concat(_blackPieces).ToList().ForEach(piece => piece.Destroy());
            _whitePieces.Clear();
            _blackPieces.Clear();
            _whitePieces.AddRange(GeneratePieces(backline, ChessColor.White, Constants.WHITE_BACKLINE_RANK));
            _blackPieces.AddRange(GeneratePieces(backline, ChessColor.Black, Constants.BLACK_BACKLINE_RANK));

            List<PieceType> pawns = Enumerable.Repeat(PieceType.Pawn, 8).ToList();
            _whitePieces.AddRange(GeneratePieces(pawns, ChessColor.White, Constants.WHITE_PAWN_STARTING_RANK));
            _blackPieces.AddRange(GeneratePieces(pawns, ChessColor.Black, Constants.BLACK_PAWN_STARTING_RANK));
        }

        private bool IsInsufficientMaterial(List<Piece> pieces)
        {
            Dictionary<PieceType, int> pieceCounts = Utilities.GetPieceTypeCounts(pieces);

            if(!pieceCounts.ContainsKey(PieceType.King))
            {
                throw new System.Exception("No king on the board for one side");
            }
            if (pieceCounts.Count == 1)
            {
                return true;
            }
            if (pieceCounts.Count == 2)
            {
                if (pieceCounts.ContainsKey(PieceType.Bishop) && pieceCounts[PieceType.Bishop] == 1)
                {
                    return true;
                }
                if (pieceCounts.ContainsKey(PieceType.Knight) && pieceCounts[PieceType.Knight] == 1)
                {
                    return true;
                }
            }
            return false;

        }

        protected override void OnMoveExecuted(Move move)
        {
            // checking for game over states

            // stalemate
            ChessColor nextTurn = Utilities.FlipTurn(_turn);

            List<Piece> pieces = nextTurn == ChessColor.White ? _whitePieces : _blackPieces;

            Dictionary<PieceType, int>  pieceCounts = Utilities.GetPieceTypeCounts(pieces);
            int moveCounts = Utilities.GetMoveCounts(pieces, this);

            if(moveCounts == 0)
            {
                GameOver(GameState.Stalemate);
                return;
            }

            // if both sides have insufficient material, it's a draw
            if (IsInsufficientMaterial(_whitePieces) && IsInsufficientMaterial(_blackPieces))
            {
                GameOver(GameState.DrawByInsufficientMaterial);
                return;
            }

            if(move.IsPawnMove || move.IsCaptureMove)
            {
                _movesWithoutPawnMoveOrCapture = 0;
                _positionCounts.Clear();
            }
            else
            {
                ++_movesWithoutPawnMoveOrCapture;
                string boardHash = GetBoardHash();
                _positionCounts[boardHash] = _positionCounts.GetValueOrDefault(boardHash, 0) + 1;
                if(_movesWithoutPawnMoveOrCapture == 100)
                {
                    GameOver(GameState.DrawByFiftyMoveRule);
                    CustomLogger.LogInfo("Draw by fifty-move rule");
                    return;
                }
                else if(_positionCounts[boardHash] == 3)
                {
                    GameOver(GameState.DrawByThreefoldRepetition);
                    CustomLogger.LogInfo("Draw by threefold repetition");
                    return;
                }
            }


            // if position has occured three times, it is a draw
            // if no pawn has moved in 50 moves, or no capture has occured in 50 moves, draw

            // after the turn, see if opponent king is in check
            (bool isCheck, bool isCheckmate) = LookForChecks(nextTurn);
            HighlightKing(isCheck, nextTurn);

            if (isCheckmate)
            {

                if (_turn == ChessColor.White)
                {
                    GameOver(GameState.CheckmateWhite);
                    CustomLogger.LogInfo("Checkmate! White wins!");
                    return;
                }
                else if (_turn == ChessColor.Black)
                {
                    GameOver(GameState.CheckmateBlack);
                    CustomLogger.LogInfo("Checkmate! Black wins!");
                    return;
                }
            }
        }

        protected override void OnMoveUndone(Move move)
        {

        }
    }
}
