using Assets.GameObjects;
using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Parts;
using Assets.Scripts.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainScript2 : MonoBehaviour
    {
        public Camera _mainCamera;

        private Board _board;

        public PieceManager _pieceManager;

        public PromotionSelector _promotionSelector;

        // Start is called before the first frame update
        void Start()
        {
            _mainCamera.enabled = true;

            

            MapPiecesToBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = _mainCamera.ScreenPointToRay( mousePos );
                Debug.Log( $"Clicked on {mousePos.x}, {mousePos.y}" );


                ray.direction = ray.direction * 100;

                if ( Physics.Raycast( ray, out RaycastHit raycastHit, 1000000f ) )
                {

                    if ( raycastHit.transform is not null )
                    {

                        int file = ( int ) ( 4 + Math.Ceiling( raycastHit.point.x ) );
                        int rank = ( int ) ( 4 + Math.Ceiling( raycastHit.point.z) );

                        Debug.Log( $"Clicked on BOARD {file}, {rank}" );

                        _board.SelectLocation( rank, file );
                        //_board.HighlightSquare( _highlightSquare, rank, file );
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
            //        Debug.Log( $"Success with {isCheck} and {isCheckmate}" );
            //    }
            //    catch ( Exception ex )
            //    {
            //        Debug.Log( $"Failed with {ex.Message}" );
            //    }
            //}

        }

        private GameObject CreateHighlightSquare()
        {
            GameObject plane = Creator.CreatePlane();
            plane.transform.position = new Vector3( 0f, 0.01f, 0f );
            return plane;
        }

        private void MapPiecesToBoard()
        {
            GameObject board = _pieceManager.CreateBoard(new Vector3(0, -0.5f, 0));

            board.AddComponent<BoxCollider>();
            //Transform[] children = _chessPieces.GetComponentsInChildren<Transform>();

            //Transform boardObject = children.Where( x => x.gameObject.name == "Board" ).Single();

            HighlightSquare highlightSquare = new( CreateHighlightSquare() );


            _board = new( Constants.BOARD_WIDTH, Constants.BOARD_HEIGHT, _pieceManager, highlightSquare, _promotionSelector );

            _board.SetupForGameStart();


            //// modify test setup if modifying this code
            //foreach ( var child in children )
            //{
            //    GameObject gamePiece = child.gameObject;

            //    // split into name and starting position
            //    string fullName = gamePiece.name;

            //    if ( fullName == "Chess Pieces" || fullName == "Board" ) continue;

            //    string type = fullName[ ..^2 ];
            //    string position = fullName[ ^2.. ];
            //    ChessColor color = position[ 1 ] == '1' || position[ 1 ] == '2' ? ChessColor.White : ChessColor.Black;
            //    Piece piece = type switch
            //    {
            //        "Pawn" => new Pawn( gamePiece, color ),
            //        "Knight" => new Knight( gamePiece, color ),
            //        "Bishop" => new Bishop( gamePiece, color ),
            //        "Rook" => new Rook( gamePiece, color ),
            //        "Queen" => new Queen( gamePiece, color ),
            //        "King" => new King( gamePiece, color ),
            //        _ => throw new InvalidOperationException( $"Cannot create a piece of type {type}" ),
            //    };
            //    (CRank rank, CFile file) = position.ReadChessNotation();
            //    _board.SetPiece( rank, file, piece );
            //}


            ////_board.MovePiece( "E2", "E4" );
            //_board.SetupForGameStart();
        }

        private void CheckForKeyPresses()
        {
            if ( Input.GetKeyDown( KeyCode.DownArrow ) )
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
