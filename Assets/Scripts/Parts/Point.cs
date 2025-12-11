using System;
using UnityEngine;

namespace Assets.Scripts.Parts
{
    public class Point : IEquatable<Point>
    {
        private CRank _rank;
        private CFile _file;


        public Point(Point point)
        {
            _rank = new CRank( point.Rank );
            _file = new CFile( point.File );
        }

        public Point( CRank rank, CFile file )
        {
            _rank = rank;
            _file = file;
            //assert(rank.Num <= 8 );
            //assert( rank.Num >= 1 );
            //assert( file.Num <= 8 );
            //assert( file.Num >= 1 );
        }

        public Point( int rank, int file )
        {
            _rank = new CRank( rank );
            _file = new CFile( file );

        }

        public CRank Rank => _rank;
        public CFile File => _file;

        public (int rank, int file) Location => ( Rank.Num, File.Num );

        // file is x, rank is y
        // (0, 0) is the location of A8 (A1, (1,1) (0,6)) (A2, (1,2), (1,6)
        // (7, 7) is the location of H1
        public Vector3 Vector => new( File.Num - 1, 8 - Rank.Num, 0);

        public void Move( int rankChange, int fileChange )
        {
            _rank.AddNum( rankChange );
            _file.AddNum( fileChange );
        }

        public bool Equals( Point other ) => _rank.Num == other.Rank.Num && _file.Num == other.File.Num;
    }
}