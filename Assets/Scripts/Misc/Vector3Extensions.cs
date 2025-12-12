using System;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public static class Vector3Extensions
    {
        public static Vector3 x_z(this Vector3 v) => new(v.x, 0f, v.z);

        public static Vector3 Lambda(this Vector3 v, Func<int, float, float> lambda)
          => new(lambda(0, v.x), lambda(1, v.y), lambda(2, v.z));

        public static Vector3 ScaledBy(this Vector3 v, float x = 1f, float y = 1f, float z = 1f)
          => new(x * v.x, y * v.y, z * v.z);

        // aka component-wise multiplication for two vectors
        public static Vector3 ScaledBy(this Vector3 v, Vector3 other)
          => new(other.x * v.x, other.y * v.y, other.z * v.z);
    }
}
