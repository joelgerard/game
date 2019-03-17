using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Soldier : MovableUnit
{


    public partial class SoliderHsm { }

    public override void Init()
    {
        base.Init();
        TotalHP = HP = 1;
    }

    public float SpeedWeight { get; }
}
