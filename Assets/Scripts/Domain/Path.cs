using System;
using UnityEngine;

public class Path : IPath
{
    public Vector2 target;

    public Path()
    {
    }

    public int Count()
    {
        return 1;
    }

    public Vector2 GetPosition(int index)
    {
        // TODO: This always returns the player base.
        Vector2 vector2 = new Vector2(8f, 0.11f);
        return vector2;
    }
}
