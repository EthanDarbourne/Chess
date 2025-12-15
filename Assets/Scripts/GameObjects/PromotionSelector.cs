using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.GameObjects
{
    public class PromotionSelector
    {

        public Camera _camera;

        private bool _isSelectorOpen = false;

        private GameObject _selector;

        private PieceType _lastSelectedPiece;

        public PromotionSelector(GameObject selector)
        {
            _selector = selector;
            _selector.transform.position = Vector3.zero;
            _lastSelectedPiece = PieceType.Empty;
            Hide();
        }

        public PieceType LastSelectedPiece => _lastSelectedPiece;

        public bool IsSelectorOpen => _isSelectorOpen;

        public IEnumerator WaitForSelection()
        {
            while(_isSelectorOpen)
            {
                yield return null;
            }
            yield return LastSelectedPiece;
        }

        private bool Between(double val, double low, double hi, double variance)
        {
            return val >= low - variance && val <= hi + variance;
        }

        public void Trigger(float x, float z)
        {
            double row = 4 - z - _selector.transform.localPosition.y;
            double col = x - _selector.transform.localPosition.x + 4;

            double start = 0d, middle = 1d, end = 2d, variance = 0.05d;

            PieceType selectedPiece = PieceType.Empty;
            if(Between(col, start, middle, variance))
            {
                if(Between(row, start, middle, variance))
                {
                    selectedPiece = PieceType.Queen;
                }
                else if(Between(row, middle, end, variance))
                {
                    selectedPiece = PieceType.Knight;
                }
            }
            else if(Between(col, middle, end, variance))
            {
                if (Between(row, start, middle, variance))
                {
                    selectedPiece = PieceType.Rook;
                }
                else if (Between(row, middle, end, variance))
                {
                    selectedPiece = PieceType.Bishop;
                }
            }
            _lastSelectedPiece = selectedPiece;
            Hide();
        }

        public void Display(Vector3 position)
        {
            position.z = 3;
            _isSelectorOpen = true;
            _selector.transform.localPosition = position;
            _selector.SetActive(true);
        }

        public void Hide()
        {
            _isSelectorOpen = false;
            _selector.SetActive( false );
        }

        public void Claim(GameObject parent)
        {
            _selector.transform.parent = parent.transform;
        }
    }
}
