using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public static class Creator
    {
        public static GameObject CreatePlane()
        {
            var plane = GameObject.CreatePrimitive( PrimitiveType.Plane );
            plane.transform.localScale = new Vector3( 0.1f, 0.1f, 0.1f );
            plane.transform.position = Vector3.zero;
            plane.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            return plane;
        }

        public static GameObject CreateColoredPlane( Color color)
        {
            var plane = CreatePlane();
            var renderer = plane.GetComponent<Renderer>();
            renderer.material = new(Shader.Find("Standard"))
            {
                color = color
            };
            return plane;
        }

        public static GameObject CreateHighlightSquare(GameObject parent)
        {
            GameObject plane = CreatePlane();
            plane.transform.localScale *= .8f;
            plane.transform.SetParent(parent.transform, false);
            return plane;
        }

        public static GameObject CreateInCheckHighlightSquare(GameObject parent)
        {
            GameObject plane = CreateColoredPlane(Color.red);
            plane.transform.SetParent(parent.transform, false);
            return plane;
        }

    }

}
