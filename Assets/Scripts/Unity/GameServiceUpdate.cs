using System;
using UnityEngine;

public struct GameServiceUpdate
{
    public GameUpdate GameUpdate;
    public Vector2 MousePos;
    public bool Click, MouseUp, MouseDown;
    public MonoBehaviour MainBehaviour;
}
