using UnityEngine;

public static class Maths
{

    public static Vector3 CalculateMoveVector(Vector3 from, Vector3 to)
    {
        return to - from;
    }

    public static float CalculateDistance2D(Vector3 from, Vector3 to)
    {
        return Vector2.Distance(new Vector2(from.x, from.z), new Vector2(to.x, to.z));

    }

    public static Vector2 toVector2WithoutY(Vector3 vector3) {
        return new Vector2(vector3.x, vector3.z);
    }



}