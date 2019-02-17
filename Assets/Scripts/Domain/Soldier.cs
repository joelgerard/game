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

    public void Move(Navigator navigator)
    {
        //shape.gameObject.transform.position =
        //                    Vector2.MoveTowards(new Vector2(shape.gameObject.transform.position.x, shape.gameObject.transform.position.y), position, 3 * Time.deltaTime);
        //
    }

    public float SpeedWeight { get; }
}
