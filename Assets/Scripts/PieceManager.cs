using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class PieceManager : MonoBehaviour
    {
        public GameObject BlackPawn;
        public GameObject BlackKnight;
        public GameObject BlackBishop;
        public GameObject BlackRook;
        public GameObject BlackQueen;
        public GameObject BlackKing;

        public GameObject WhitePawn;
        public GameObject WhiteKnight;
        public GameObject WhiteBishop;
        public GameObject WhiteRook;
        public GameObject WhiteQueen;
        public GameObject WhiteKing;

        public GameObject Board;

        public GameObject PromotionSelector;

        public GameObject PieceGraveyard;

        // hold a reference of all gameobjects to delete them later
        public List<GameObject> _gameObjects;

        public PieceManager( GameObject blackPawn, GameObject blackKnight, GameObject blackBishop, GameObject blackRook, GameObject blackQueen, GameObject blackKing, GameObject whitePawn, GameObject whiteKnight, GameObject whiteBishop, GameObject whiteRook, GameObject whiteQueen, GameObject whiteKing, GameObject board )
        {
            BlackPawn = blackPawn;
            BlackKnight = blackKnight;
            BlackBishop = blackBishop;
            BlackRook = blackRook;
            BlackQueen = blackQueen;
            BlackKing = blackKing;
            WhitePawn = whitePawn;
            WhiteKnight = whiteKnight;
            WhiteBishop = whiteBishop;
            WhiteRook = whiteRook;
            WhiteQueen = whiteQueen;
            WhiteKing = whiteKing;
            Board = board;
        }

        private GameObject InstantiateFromPos( GameObject gameObject, Vector3 position, GameObject? parent = null )
        {
            GameObject ret;
            ret = Instantiate(gameObject, position, gameObject.transform.rotation);
            if ( parent is not null )
            {
                ret.transform.SetParent( parent.transform, false );
                ret.transform.localRotation = Quaternion.identity;
                CustomLogger.LogDebug( $"Setting parent of {ret.name} to {parent.name} with rotation {ret.transform.rotation}" );
            }
            _gameObjects.Add(ret);
            return ret;
        }

        public GameObject CreateBoard(Vector3 position) => InstantiateFromPos( Board, position );

        public GameObject CreatePieceGraveyard(Vector3 position) => InstantiateFromPos(PieceGraveyard, position);

        public GameObject CreatePromotionSelector()
        {
            GameObject ret = Instantiate( PromotionSelector, Vector3.zero, new Quaternion( 0, -0.707106829f, -0.707106829f, 0 ) );
            ret.transform.localScale = new Vector3( 0.5f, 0.5f, 1 );
            return ret;
        }

        public GameObject GeneratePiece(PieceType type, ChessColor color, GameObject board) 
            => InstantiateFromPos( GetPiece( type, color ), Vector3.zero, board);

        private GameObject GetPiece(PieceType type, ChessColor color)
        {
            if ( color == ChessColor.White )
            {
                return type switch
                {
                    PieceType.Pawn => WhitePawn,
                    PieceType.Knight => WhiteKnight,
                    PieceType.Bishop => WhiteBishop,
                    PieceType.Rook => WhiteRook,
                    PieceType.Queen => WhiteQueen,
                    PieceType.King => WhiteKing,
                    _ => throw new NotSupportedException( $"Cannot find piece {type} and color {color}" )
                };
            }
            else if ( color == ChessColor.Black )
            {
                return type switch
                {
                    PieceType.Pawn => BlackPawn,
                    PieceType.Knight => BlackKnight,
                    PieceType.Bishop => BlackBishop,
                    PieceType.Rook => BlackRook,
                    PieceType.Queen => BlackQueen,
                    PieceType.King => BlackKing,
                    _ => throw new NotSupportedException( $"Cannot find piece {type} and color {color}" )
                };
            }
            throw new NotSupportedException( $"Cannot find piece {type} and color {color}" );
        }

        public void ClearObject(GameObject gameObject)
        {
            _gameObjects.Remove( gameObject );
            Destroy( gameObject );
        }

        public void ClearAllObjects()
        {
            foreach(var gameObj in _gameObjects)
            {
                Destroy( gameObj );
            }
            _gameObjects.Clear();
        }
    }
}