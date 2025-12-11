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
    public class ChessVsConnect4Board : Board
    {
        public ChessVsConnect4Board(int width, int height, PieceManager pieceManager, PieceGraveyard[] pieceGraveyards, GameObject boardObject, HighlightSquare defaultHighlightSquare, HighlightSquare inCheckHighlightSquare, PromotionSelector selector, MonoBehaviour monoBehaviour) : base(width, height, pieceManager, pieceGraveyards, boardObject, defaultHighlightSquare, inCheckHighlightSquare, selector, monoBehaviour)
        {
        }

        protected override void SetupBoard()
        {
            _whitePieces.AddRange(GeneratePieces(Constants.DEFAULT_BACKLINE, ChessColor.White, Constants.WHITE_BACKLINE_RANK));

            List<PieceType> pawns = Enumerable.Repeat(PieceType.Pawn, 8).ToList();
            _whitePieces.AddRange(GeneratePieces(pawns, ChessColor.White, Constants.WHITE_PAWN_STARTING_RANK));

            for (int i = 0; i < 16; ++i)
            {
                _blackPieceGraveyard.SendPieceToGraveyard(GeneratePiece(PieceType.Connect4, ChessColor.Black));
            }
        }

        protected override void DoPlayerInput(int rank, int file)
        {
            if (_movePieceCoroutine is not null) // todo: could cancel here
            {
                CustomLogger.LogDebug("A movement is already in progress, ignoring input");
                return;
            }
            if (_turn == ChessColor.White)
            {
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
            else
            {
                PlaceConnect4Piece(file);
            }
        }

        public override bool IsValidPositionAfterMove(Move move)
        {
            if(move.To.Piece is not null && move.To.Piece.Type == PieceType.Connect4)
            {
                return false;
            }
            return true;
        }

        private void PlaceConnect4Piece(int file)
        {
            if (GetSquare(Height, file).Piece is not null)
            {
                CustomLogger.LogInfo("Cannot place piece in this file");
                return;
            }
            int targetRank = Height;
            while (targetRank > 1 && GetSquare(targetRank - 1, file).Piece is null)
            {
                targetRank -= 1;
            }
            Piece? piece = _blackPieceGraveyard.RevivePiece(PieceType.Connect4);
            if (piece is null)
            {
                CustomLogger.LogDebug("No Connect4 pieces available to place");
                GameOver(GameState.WhiteWin);
                return;
            }
            ClaimPiece(piece);
            _blackPieces.Add(piece);
            CustomLogger.LogDebug("Moving Connect4 piece to location");
            SetPiece(targetRank, file, piece);
            SwapTurn();
        }

        protected override void OnMoveExecuted()
        {
            // move connect 4 pieces down
            // check for 4 in a row

            for(int file = 1; file <= _width; ++file)
            {
                for(int rank = 1; rank <= 8; ++rank)
                {
                    Square currentSquare = GetSquare(rank, file);
                    if (currentSquare.Piece is null || currentSquare.Piece.Type != PieceType.Connect4) continue;
                    // try to move piece down
                    int targetRank = rank;
                    while (targetRank > 1 && GetSquare(targetRank - 1, file).Piece is null)
                    {
                        targetRank -= 1;
                    }
                    if (targetRank < rank)
                    {
                        Piece curPiece = currentSquare.RemovePiece();
                        GetSquare(targetRank, file).SetPiece(curPiece);
                    }
                }
            }

            if (_blackPieces.Any(piece => CheckForFourInARowAroundLocation(piece.Location.Location.rank, piece.Location.Location.file)))
            {
                GameOver(GameState.BlackWin);
            }

        }

        private bool IsConnect4Piece(int rank, int file)
        {
            return GetSquareOrDefault(rank, file)?.Piece?.Type == PieceType.Connect4;
        }

        private bool CheckForFourInARowAroundLocation(int rank, int file)
        {
            if(!IsConnect4Piece(rank, file))
            {
                CustomLogger.LogDebug("No connect4 piece here");
                return false;
            }

            bool posRank = true, negRank = true, posFile = true, negFile = true;
            for(int i = 1; i <= 3; ++i)
            {
                posRank &= IsConnect4Piece(rank + i, file);
                negRank &= IsConnect4Piece(rank - i, file);
                posFile &= IsConnect4Piece(rank, file + i);
                negFile &= IsConnect4Piece(rank, file - i);
            }

            return posRank || negRank || posFile || negFile;

        }
    }
}
