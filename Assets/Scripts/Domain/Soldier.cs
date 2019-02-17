using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier
{
    GameObject go;

    public Soldier(GameObject go)
    {
        this.go = go;
    }

    public Vector2 Position
    {
        get
        { 
            return this.go.transform.position; 
        }
        set
        {
            this.go.transform.position = value;
        }
    }

    public float SpeedWeight { get; }
}
