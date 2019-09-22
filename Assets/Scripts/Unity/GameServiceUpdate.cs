using System;
using UnityEngine;

public struct GameServiceUpdate
{
    public Vector2 MousePos;
    public bool Click, MouseUp, MouseDown;
    public MonoBehaviour MainBehaviour;
    public float DeltaTime;
}
