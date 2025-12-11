using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class Chess960Board : ChessBoard
    {
        public Chess960Board(int width, int height, PieceManager pieceManager, PieceGraveyard[] pieceGraveyards, GameObject boardObject, HighlightSquare defaultHighlightSquare, HighlightSquare inCheckHighlightSquare, PromotionSelector selector, MonoBehaviour monoBehaviour) : base(width, height, pieceManager, pieceGraveyards, boardObject, defaultHighlightSquare, inCheckHighlightSquare, selector, monoBehaviour)
        {
        }

        protected override void SetupBoard()
        {
            List<PieceType> backline = Chess960Randomizer.GetRandomBackLine();
            MovePiecesToStartSquares(backline);
        }

    }
}