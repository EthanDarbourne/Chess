#nullable enable

using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Pieces;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class PieceGraveyard
    {

        private readonly Piece?[] _capturedPieces = new Piece[Constants.NUM_PIECES];
        private readonly Vector3[] _capturedPiecePositions = new Vector3[Constants.NUM_PIECES];

        private GameObject _pieceGraveyardObj;

        private ChessColor _graveyardColor;

        public PieceGraveyard( GameObject pieceGraveyardObj, ChessColor color )
        {
            _pieceGraveyardObj = pieceGraveyardObj;
            _graveyardColor = color;
            Vector3 startPos = Vector3.zero;
            for (int i = 0; i < 16; ++i)
            {
                if (i == Constants.NUM_PIECES / 2) // two lines of 8
                {
                    startPos.x += 1;
                    startPos.y = 0;
                }
                _capturedPiecePositions[i] = startPos;
                startPos.y += 1;
            }
        }

        public void SendPieceToGraveyard(Piece piece)
        {
            if(piece.Color != _graveyardColor)
            {
                CustomLogger.LogError($"Tried to send piece to graveyard {_graveyardColor} with wrong color: {piece.Color}");
                throw new System.Exception();
            }
            piece.SetParent( _pieceGraveyardObj );
            for (int i = 0; i < Constants.NUM_PIECES; ++i)
            {
                if (_capturedPieces[i] is null)
                {
                    _capturedPieces[i] = piece;
                    piece.SetGraveyardLocation(_capturedPiecePositions[i]);
                    return;
                }
            }
        }

        public Piece? RevivePiece(Piece piece)
        {
            int index = Constants.NUM_PIECES - 1;
            while (index >= 0)
            {
                if (_capturedPieces[index] is not null &&
                    _capturedPieces[index] == piece)
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

        public Piece? RevivePiece(PieceType type)
        {
            int index = Constants.NUM_PIECES - 1;
            while (index >= 0)
            {
                if (_capturedPieces[index] is not null &&
                    _capturedPieces[index]!.Type == type)
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
