using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSoldierEvent : UnityGameEvent
{
    public Vector2 pos;
    public Allegiance allegiance;

    public AddSoldierEvent(Vector2 pos, Allegiance allegiance)
    {
        this.pos = pos;
        this.allegiance = allegiance;
    }
}
