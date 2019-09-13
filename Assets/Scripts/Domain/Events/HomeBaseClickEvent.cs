using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBaseClickEvent : GameEvent
{
    public Vector2 pos;

    public HomeBaseClickEvent(Vector2 pos)
    {
        this.pos = pos;
    }
}
