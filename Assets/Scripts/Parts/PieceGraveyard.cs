#nullable enable

using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Pieces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class PieceGraveyard
    {

        private readonly Piece?[] _capturedPieces = new Piece[Constants.NUM_PIECES];
        private readonly Vector3[] _capturedPiecePositions = new Vector3[Constants.NUM_PIECES];

        private GameObject _pieceGraveyardObj;

        public PieceGraveyard( GameObject pieceGraveyardObj )
        {
            _pieceGraveyardObj = pieceGraveyardObj;
            Vector3 startPos = new Vector3(0.5f, 0f, 0.5f);
            for (int i = 0; i < 16; ++i)
            {
                if (i == Constants.NUM_PIECES / 2) // two lines of 8
                {
                    startPos.x += 1;
                    startPos.z = 0;
                }
                startPos.z += 1;
                _capturedPiecePositions[i] = startPos;
            }
            CustomLogger.LogDebug(_capturedPiecePositions[0].ToString());
            CustomLogger.LogDebug(_capturedPiecePositions[1].ToString());
        }

        public void SendPieceToGraveyard(Piece piece)
        {
            for (int i = 0; i < Constants.NUM_PIECES; ++i)
            {
                if (_capturedPieces[i] is null)
                {
                    _capturedPieces[i] = piece;
                    _pieceGraveyardObj.transform.parent = _pieceGraveyardObj.transform;
                    piece.SetGraveyardLocation(_capturedPiecePositions[i]);
                    return;
                }
            }
        }

        public Piece? RevivePiece(PieceType type, ChessColor color)
        {
            int index = Constants.NUM_PIECES - 1;
            while (index >= 0)
            {
                if (_capturedPieces[index] is not null &&
                    _capturedPieces[index]!.Type == type &&
                    _capturedPieces[index]!.Color == color)
                {
                    break;
                }
                --index;
            }

            if (index == -1)
            {
                return null;
            }

            Piece? revivedPiece = _capturedPieces[index];
            _capturedPieces[index] = null;

            return revivedPiece;
        }
    }
}
