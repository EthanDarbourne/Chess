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
            return plane;
        }
    }
}
