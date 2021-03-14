using UnityEngine;

public static class ExtensionMethod
{
    private static Vector3 v3;

    public static Vector3 x(this Vector3 value, float x)
    {
        v3 = value;
        v3.x = x;
        return v3;
    }

    public static Vector3 y(this Vector3 value, float y)
    {
        v3 = value;
        v3.y = y;
        return v3;
    }

    public static Vector3 z(this Vector3 value, float z)
    {
        v3 = value;
        v3.z = z;
        return v3;
    }

    public static Vector3 AdjX(this Vector3 value, float x)
    {
        v3 = value;
        v3.x += x;
        return v3;
    }

    public static Vector3 AdjY(this Vector3 value, float y)
    {
        v3 = value;
        v3.y += y;
        return v3;
    }

    public static Vector3 AdjZ(this Vector3 value, float z)
    {
        v3 = value;
        v3.z += z;
        return v3;
    }
}
