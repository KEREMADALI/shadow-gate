using UnityEngine;
using System.Collections;

public static class TransformExtensions
{
    public static float DistanceTo(this Transform original, Transform target) 
    {
        if (original == null || target == null) 
        {
            throw new System.Exception("Transforms can not be null in Transform.DistanceTo() method!");
        }

        return original.position.DistanceTo(target.position);
    }

    /// <summary>
    /// This is better in performace since taking square root is an expensive process
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float SquareDistanceTo(this Transform original, Transform target) 
    {
        if (original == null || target == null)
        {
            throw new System.Exception("Transforms can not be null in Transform.DistanceTo() method!");
        }

        return Mathf.Pow((original.position.x - target.position.x), 2) 
            + Mathf.Pow((original.position.y - target.position.y), 2)
            + Mathf.Pow((original.position.z - target.position.z), 2);
    }
}
