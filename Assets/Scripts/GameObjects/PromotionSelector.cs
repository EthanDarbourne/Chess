using Assets.Scripts.Enums;
using Assets.Scripts.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameObjects
{
    public class PromotionSelector : MonoBehaviour
    {

        public Camera _camera;

        private bool _isSelectorOpen = false;

        public GameObject Selector;

        private PieceType? _lastSelectedPiece;


        public PromotionSelector()
        {

        }

        public PieceType? LastSelectedPiece => _lastSelectedPiece;

        public void Update()
        {
            if(!_isSelectorOpen)
            {
                return;
            }

            if ( Input.GetMouseButtonDown( 0 ) )
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = _camera.ScreenPointToRay( mousePos );
                Debug.Log( $"Clicked on {mousePos.x}, {mousePos.y}" );


                ray.direction = ray.direction * 100;

                if ( Physics.Raycast( ray, out RaycastHit raycastHit, 1000000f ) && raycastHit.transform is not null )
                {
                    Debug.Log( $"{raycastHit.point.x}, { raycastHit.point.z }" );
                    int file = ( int ) ( 4 + Math.Ceiling( raycastHit.point.x ) );
                    int rank = ( int ) ( 4 + Math.Ceiling( raycastHit.point.z ) );
                    // calculate selected piece
                    // set piece to that
                    // and hide
                }
                else
                {
                    // clicked out of selector, cancel promotion
                    Hide();
                }
            }
        }

        public void Display(Vector3 position)
        {
            _isSelectorOpen = true;
            Selector.transform.position = position;
            Selector.SetActive(true);
        }

        public void Hide()
        {
            _isSelectorOpen = false;
            Selector.SetActive( false );
        }
    }
}
