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

        public Square( Point point, GameObject moveToHighlight )
        {
            _location = point;
            _moveToHighlight = moveToHighlight;
            _moveToHighlight.transform.localPosition = _location.Vector + CommonVectors.FirstLayerHeightOffset + CommonVectors.CentreOffset;
        }

        public bool IsCapturable( ChessColor color ) => _piece is not null && _piece.Color != color;

        public bool IsFree => _piece is null || _piece.Type == PieceType.Connect4; // empty square

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

        public Piece CapturePiece( Board board )
        {
            Assert.IsNotNull( _piece );
            Piece ret = _piece;
            board.OnPieceCaptured( ret );
            return ret;
        }

        // capture the piece at this square and replace it with incoming piece
        public Piece CapturePiece( Piece captor, Board board )
        {
            Piece ret = CapturePiece(board);
            MovePieceTo( captor );
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
            plane.TranslateTo( Point.Vector );
        }

        public void EnableMoveToHighlight()
        {
            _moveToHighlight.SetActive( true );
        }

        public void DisableMoveToHighlight()
        {
            _moveToHighlight.SetActive( false );
        }

        public void Destroy()
        {
            Object.Destroy( _moveToHighlight );
        }

        public Piece? Piece => _piece;

        public Point Point => _location;
        public CRank Rank => _location.Rank;
        public CFile File => _location.File;
    }
}