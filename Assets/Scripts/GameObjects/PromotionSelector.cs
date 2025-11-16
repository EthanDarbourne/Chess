using Assets.Scripts.Enums;
using Assets.Scripts.Misc;
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
            Hide();
            yield return LastSelectedPiece;
        }

        public void Trigger(float x, float z)
        {
            double row = (x - _selector.transform.position.x);
            double col = (z - _selector.transform.position.z);

            double left = -0.83d;
            double right = 1.28d;
            double top = 0.98d;
            double bottom = -1.16d;

            if(row < left || row > right || row < bottom || row > top)
            {
                _lastSelectedPiece = PieceType.Empty;
            }
            else if(row - left < right - row)
            {
                if(top - col < col - bottom)
                {
                    _lastSelectedPiece = PieceType.Queen;
                }
                else
                {
                    _lastSelectedPiece = PieceType.Knight;
                }
            }
            else
            {
                if ( top - col < col - bottom )
                {
                    _lastSelectedPiece = PieceType.Rook;
                }
                else
                {
                    _lastSelectedPiece = PieceType.Bishop;
                }
            }
            CustomLogger.LogInfo( $"Selected {_lastSelectedPiece}" );
            Hide();
        }

        public void Display(Vector3 position)
        {
            position.y = 3;
            _isSelectorOpen = true;
            _selector.transform.position = position;
            _selector.SetActive(true);
        }

        public void Hide()
        {
            _isSelectorOpen = false;
            _selector.SetActive( false );
        }
    }
}
