using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public static class CommonVectors
    {
        public static Vector3 FirstLayerHeightOffset = new(0, 0, 0.02f);
        public static Vector3 SecondLayerHeightOffset = new(0, 0, 0.01f);
        public static Vector3 CentreOffset = new(0.5f, 0.5f, 0);

        public static Vector3 IdentityVector = new(1f, 1f, 1f);
    }
}
