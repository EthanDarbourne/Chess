using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
using Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Parts
{
    public class Square
    {
        private Point _location;
        private Piece? _piece = null;
        private GameObject _moveToHighlight;
        private static Vector3 _centerOffset = new( -0.5f, 0, -0.5f );
        private static Vector3 _heightOffset = new( 0, 0.01f, 0 );

        public Square( Point point )
        {
            _location = point;
            _moveToHighlight = Creator.CreatePlane();
            _moveToHighlight.transform.position = _location.Vector + _centerOffset + _heightOffset;
            _moveToHighlight.transform.localScale *= 0.2f;
        }

        public bool IsCapturable( ChessColor color ) => _piece is not null && _piece.Color != color;

        public bool IsFree => _piece is null; // empty square

        // The color attempting to move here
        public bool IsFreeOrCapturable( ChessColor color )
        {
            return IsFree || IsCapturable( color );
        }

        public void SetPiece( Piece piece )
        {
            _piece = piece;
            _piece.SetLocation( new( _location ) ); // set the initial and current location of the piece
        }

        public void MovePieceTo( Piece piece )
        {
            _piece = piece;
            _piece.MoveTo( new( _location ) );
        }

        public Piece CapturePiece()
        {
            Assert.IsNotNull( _piece );
            Piece ret = _piece;
            _piece.Delete();
            return ret;
        }

        public Piece CapturePiece( Piece piece )
        {
            Piece ret = CapturePiece();
            MovePieceTo( piece );
            return ret;
        }

        public Piece? RemovePiece()
        {
            Piece? retPiece = _piece;
            _piece = null;
            return retPiece;
        }

        public void HighlightSquare( HighlightSquare plane )
        {
            plane.Show();
            plane.TranslateTo( Point.Vector + _centerOffset );
        }

        public void EnableMoveToHighlight()
        {
            _moveToHighlight.SetActive( true );
        }

        public void DisableMoveToHighlight()
        {
            _moveToHighlight.SetActive( false );
        }

        public Piece? Piece => _piece;

        public Point Point => _location;
        public CRank Rank => _location.Rank;
        public CFile File => _location.File;
    }
}