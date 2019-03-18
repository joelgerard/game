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
        return target;


    }
}
