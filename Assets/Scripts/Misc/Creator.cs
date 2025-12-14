using UnityEngine;

namespace Assets.Scripts.Misc
{
    public static class Creator
    {
        public static GameObject CreatePlane(GameObject parent)
        {
            var plane = GameObject.CreatePrimitive( PrimitiveType.Plane );
            plane.transform.SetParent(parent.transform, false);
            plane.transform.localScale = new Vector3( 0.1f, 0.1f, 0.1f );
            plane.transform.position = Vector3.zero;
            plane.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            return plane;
        }

        public static GameObject CreateColoredPlane( Color color, GameObject parent)
        {
            var plane = CreatePlane(parent);
            var renderer = plane.GetComponent<Renderer>();
            renderer.material = new(Shader.Find("Standard"))
            {
                color = color
            };
            return plane;
        }

        public static GameObject CreateHighlightSquare(GameObject parent)
        {
            GameObject plane = CreatePlane(parent);
            plane.transform.localScale *= .8f;
            
            return plane;
        }

        public static GameObject CreateInCheckHighlightSquare(GameObject parent)
        {
            GameObject plane = CreateColoredPlane(Color.red, parent);
            plane.transform.SetParent(parent.transform, false);
            return plane;
        }

        public static GameObject CreateFileHighlight(GameObject parent)
        {
            GameObject plane = CreatePlane(parent);
            plane.transform.SetParent(parent.transform, false);
            //plane.transform.localScale = plane.transform.localScale.ScaledBy(0.1f);
            return plane;
        }
    }
}
