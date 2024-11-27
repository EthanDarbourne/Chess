﻿using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using Assets.Scripts.ShallowCopy;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.Pieces
{
    public abstract class Piece
    {
        protected Point _location;
        protected Point _initialLocation;
        protected readonly ChessColor _color;
        protected GameObject _gamePiece;

        protected Piece( GameObject gamePiece, CRank rank, CFile file, ChessColor color )
        {
            _gamePiece = gamePiece;
            _location = new( rank, file );
            _initialLocation = new Point( _location );
            _color = color;
        }

        protected Piece( GameObject gamePiece, ChessColor color )
        {
            _gamePiece = gamePiece;
            _location = new Point( 0, 0 );
            _initialLocation = new Point( _location );
            _color = color;
        }

        public ChessColor Color => _color;

        public Point Location => _location;

        public abstract PieceType Type { get; }

        protected abstract List<Move> GetPotentialMoves( Board board );

        public abstract ShallowPiece CreateShallowPiece();

        public List<Move> GetValidMoves( Board board )
        {
            List<Move> potentialMoves = GetPotentialMoves( board );
            return potentialMoves.Where( move => board.IsValidPositionAfterMove( move ) ).ToList();
        }

        // - changes moves positively along the files
        // - changes moves positively along the ranks
        public virtual void Move( int rankChange, int fileChange )
        {
            _location.Move( rankChange, fileChange );
            // file is x, rank is y
            _gamePiece.transform.position += new Vector3( fileChange, 0, rankChange );
        }

        public void MoveTo( Point point )
        {
            int rankChange = point.Rank.Num - _location.Rank.Num;
            int fileChange = point.File.Num - _location.File.Num;
            Move( rankChange, fileChange );
        }

        // sets the initial and current location of the piece
        public void SetLocation( Point point )
        {
            _location = point;
            _initialLocation = new( _location );
            // set the location of the gameobject
            // (3.5, 3.5) is the location of A1
            // (3.5, -3.5) is the location of H1
            // to move positive file, move negative x
            // to move positive rank, move negative z
            _gamePiece.transform.position = new Vector3( point.File.Num - 4.5f, 0, point.Rank.Num - 4.5f);
        }

        public void SetLocation( CRank rank, CFile file )
        {
            SetLocation( new( rank, file ) );
        }

        public void Uncapture()
        {
            _gamePiece.SetActive( true );
        }

        public void Delete()
        {
            _gamePiece.SetActive( false );
        }
    }
}