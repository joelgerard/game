using UnityEngine;

public interface IPath
{
    Vector2 GetPosition(int index);
    int Count();
}