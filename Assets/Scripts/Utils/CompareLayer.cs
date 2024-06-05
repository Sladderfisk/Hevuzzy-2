using UnityEngine;

public static class Utils
{
    public static bool CompareLayer(LayerMask a, int b)
    {
        return (1 << a) == b;
    }
}