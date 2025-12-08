using Assets.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public static class Chess960Randomizer
    {

        private static int GetRandom(List<int> indexes)
        {
            int len = indexes.Count;
            int randomIndex = Random.Range(0, len);
            return indexes[randomIndex];
        }

        private static List<PieceType> GetChess960Setup()
        {
            List<PieceType> pieceLocations = Enumerable.Repeat(PieceType.Empty, 8).ToList();

            // place two bishops
            int blackBishopIndex = GetRandom(new List<int> { 0, 2, 4, 6 });
            int whiteBishopIndex = GetRandom(new List<int> { 1, 3, 5, 7 });

            List<int> indexes = Enumerable.Range(0, 8).ToList();

            indexes.Remove(blackBishopIndex);
            indexes.Remove(whiteBishopIndex);

            // place queen

            int queenIndex = GetRandom(indexes);
            indexes.Remove(queenIndex);

            // place knights
            int firstKnightIndex = GetRandom(indexes);
            indexes.Remove(firstKnightIndex);
            int secondKnightIndex = GetRandom(indexes);
            indexes.Remove(secondKnightIndex);

            // place rooks and king

            pieceLocations[blackBishopIndex] = PieceType.Bishop;
            pieceLocations[whiteBishopIndex] = PieceType.Bishop;
            pieceLocations[queenIndex] = PieceType.Queen;
            pieceLocations[firstKnightIndex] = PieceType.Knight;
            pieceLocations[secondKnightIndex] = PieceType.Knight;
            pieceLocations[indexes[0]] = PieceType.Rook;
            pieceLocations[indexes[1]] = PieceType.King;
            pieceLocations[indexes[2]] = PieceType.Rook;

            return pieceLocations;
        }

        public static List<PieceType> GetRandomBackLine(int? seed = null)
        {
            if(seed.HasValue)
            {
                Random.InitState(seed.Value);
            }
            return GetChess960Setup();
        }

    }
}
