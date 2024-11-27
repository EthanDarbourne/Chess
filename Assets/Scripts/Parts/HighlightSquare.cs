using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Parts
{

    public class HighlightSquare
    {

        private readonly GameObject _highlightPlane;


        public HighlightSquare( GameObject highlightPlane )
        {
            _highlightPlane = highlightPlane;
        }

        public Vector3 Position => _highlightPlane.transform.position;

        public void TranslateTo(Vector3 position)
        {
            position.y = 0.01f;
            _highlightPlane.transform.position = position;
        }

        public void Show()
        {
            _highlightPlane.gameObject.SetActive( true );
        }

        public void Hide()
        {
            _highlightPlane.gameObject.SetActive( false );
        }
    }
}
