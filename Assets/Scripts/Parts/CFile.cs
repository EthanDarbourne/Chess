using System;
using Unity.VisualScripting;

namespace Assets.Scripts.Parts
{
    public class CFile
    {
        private int _file;

        public CFile( CFile file )
        {
            _file = file.Num;
        }

        public CFile( int file )
        {
            _file = file;
        }

        public int Num => _file;

        public int AddNum( int change )
        {
            _file += change;
            return _file;
        }

        public static int operator -( CFile a, CFile b ) => a.Num - b.Num;
    }

    public static class CFileExtensions
    {
        public static bool Equals( this CFile a, int b ) => a.Num == b;
    }
}