﻿using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainScript : MonoBehaviour
    {

        public GameObject _chessPieces;

        private Board _board;

        public Camera _mainCamera;
        public Camera _secondCamera;

        public Plane _plane;
        private HighlightSquare _highlightSquare;

        private int LastMove = -1;

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera.enabled = true;
            MapPiecesToBoard();

            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = _mainCamera.ScreenPointToRay( mousePos );

                ray.direction = ray.direction * 100;

                if ( Physics.Raycast( ray, out RaycastHit raycastHit, 1000000f ) )
                {
                    
                    if ( raycastHit.transform is not null )
                    {
                        int file = (1 + ( int ) raycastHit.point.x);
                        int rank = (1 + ( int ) raycastHit.point.z);

                        _board.SelectLocation( rank, file );
                        //_board.HighlightSquare( _highlightSquare, rank, file );
                    }
                }
            }
            CheckForKeyPresses();

            if(LastMove != _board.MoveCount)
            {
                LastMove = _board.MoveCount;
                try
                {
                    (bool isCheck, bool isCheckmate) = _board.LookForChecks( _board.Turn );
                    Debug.Log( $"Success with {isCheck} and {isCheckmate}" );
                }
                catch (Exception ex)
                {
                    Debug.Log( $"Failed with {ex.Message}" );
                }
            }

        }

        private GameObject CreateHighlightSquare()
        {
            GameObject plane = Creator.CreatePlane();
            plane.transform.position = new Vector3( 0.5f, 0.01f, 0.5f );
            return plane;
        }

        private void MapPiecesToBoard()
        {
            Transform[] children = _chessPieces.GetComponentsInChildren<Transform>();

            Transform boardObject = children.Where( x => x.gameObject.name == "Board" ).Single();

            _highlightSquare = new HighlightSquare( CreateHighlightSquare() );
            _board = new( Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT, boardObject.gameObject, _highlightSquare );

            // modify test setup if modifying this code
            foreach ( var child in children )
            {
                GameObject gamePiece = child.gameObject;

                // split into name and starting position
                string fullName = gamePiece.name;

                if ( fullName == "Chess Pieces" || fullName == "Board" ) continue;

                string type = fullName[ ..^2 ];
                string position = fullName[ ^2.. ];
                ChessColor color = position[ 1 ] == '1' || position[ 1 ] == '2' ? ChessColor.White : ChessColor.Black;
                Piece piece = type switch
                {
                    "Pawn" => new Pawn( gamePiece, color ),
                    "Knight" => new Knight( gamePiece, color ),
                    "Bishop" => new Bishop( gamePiece, color ),
                    "Rook" => new Rook( gamePiece, color ),
                    "Queen" => new Queen( gamePiece, color ),
                    "King" => new King( gamePiece, color ),
                    _ => throw new InvalidOperationException( $"Cannot create a piece of type {type}" ),
                };
                (CRank rank, CFile file) = position.ReadChessNotation();
                _board.SetPiece( rank, file, piece );
            }


            //_board.MovePiece( "E2", "E4" );
            _board.SetupForGameStart();
        }

        private void CheckForKeyPresses()
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                // game to end
                _board.ExecuteAllMoves();
            }
            else if ( Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                // game to start
                _board.UndoAllMoves();
            }
            else if ( Input.GetKeyDown( KeyCode.LeftArrow ) )
            {
                // back one move
                _board.UndoOneMove();
            }
            else if ( Input.GetKeyDown( KeyCode.RightArrow ) )
            {
                // forward one move
                _board.ExecuteOneMove();
            }
        }
        
    }
}
