using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Misc
{
    public static class Constants
    {


        public static int MIN_BOARD_HEIGHT = 1;
        public static int MAX_BOARD_HEIGHT = 8;
        public static int MIN_BOARD_WIDTH = 1;
        public static int MAX_BOARD_WIDTH = 8;
        public static int BOARD_WIDTH = 8;
        public static int BOARD_HEIGHT = 8;


        public static int WHITE_PAWN_DIRECTION = 1;
        public static int BLACK_PAWN_DIRECTION = -1;
        public static int WHITE_PAWN_STARTING_RANK = 2;
        public static int BLACK_PAWN_STARTING_RANK = 7;
        public static int WHITE_PAWN_ENPASSANT_RANK = 5;
        public static int BLACK_PAWN_ENPASSANT_RANK = 4;
        public static int WHITE_PAWN_PROMOTION_RANK = 8;
        public static int BLACK_PAWN_PROMOTION_RANK = 1;

        public static readonly List<string> InitialPiecePositions = new() { "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2", "Ra1", "Nb1", "Bc1", "Qd1", "Ke1", "Bf1", "Ng1", "Rh1", "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7", "Ra8", "Nb8", "Bc8", "Qd8", "Ke8", "Bf8", "Ng8", "Rh8" };
    }
}
