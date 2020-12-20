using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtension
{
    public static Color With(this Color original
        , float? r = null
        , float? g = null
        , float? b = null
        , float? a = null)
    {
        var newR = r.HasValue ? (float)r : original.r;
        var newG = g.HasValue ? (float)g : original.g;
        var newB = b.HasValue ? (float)b : original.b;
        var newA = a.HasValue ? (float)a : original.a;

        return new Color(newR, newG, newB, newA);
    }
}
