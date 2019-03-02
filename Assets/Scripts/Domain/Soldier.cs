using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Soldier : MovableUnit
{


    public partial class SoliderHsm { }



    //private Hsm.StateValue<int> StateValue_HP = new Hsm.StateValue<int>(100);



    public Soldier(GameObject go)
    {
        this.go = go;

    }








    public float SpeedWeight { get; }
}
