using Assets.Scripts.Enums;
using Assets.Scripts.Moves;
using Assets.Scripts.Parts;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ShallowCopy
{
    public abstract class ShallowPiece
    {
        protected int _rank;
        protected int _file;
        protected readonly ChessColor _color;

        protected ShallowPiece( int rank, int file, ChessColor color )
        {
            _rank = rank;
            _file = file;
            _color = color;
        }

        protected ShallowPiece( ChessColor color )
        {
            _rank = 0;
            _file = 0;
            _color = color;
        }

        public ChessColor Color => _color;
        public int Rank => _rank;
        public int File => _file;

        public abstract PieceType Type { get; }

        protected abstract List<ShallowMove> GetPotentialMoves( ShallowBoard board );

        public List<ShallowMove> GetValidMoves( ShallowBoard board )
        {
            List<ShallowMove> potentialMoves = GetPotentialMoves( board );
            return potentialMoves.Where( move => board.IsValidPositionAfterMove( move ) ).ToList();
        }

        public virtual void Move( int rankChange, int fileChange )
        {
            _rank += rankChange;
            _file += fileChange;
        }
    }

}
