using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Soldier : MovableUnit
{
    GameObject go;

    public partial class SoliderHsm { }



    //private Hsm.StateValue<int> StateValue_HP = new Hsm.StateValue<int>(100);



    public Soldier(GameObject go)
    {
        this.go = go;

    }




    // TODO: Move to unit
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
