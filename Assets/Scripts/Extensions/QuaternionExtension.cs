using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Extensions
{
    public static class QuaternionExtension
    {
        public static Quaternion With(this Quaternion original
            , float? x = null
            , float? y = null
            , float? z = null
            , float? w = null)
        {
            var newX = x.HasValue ? (float)x : original.x;
            var newY = y.HasValue ? (float)y : original.y;
            var newZ = z.HasValue ? (float)z : original.z;
            var newW = w.HasValue ? (float)w : original.w;

            return new Quaternion(newX, newY, newZ, newW);
        }
    }
}