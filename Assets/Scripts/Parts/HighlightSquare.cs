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
            Debug.Log( "Moving highlight to:" + position );
            position.y = 0.01f;
            _highlightPlane.transform.position = ( position );
        }
    }
}
