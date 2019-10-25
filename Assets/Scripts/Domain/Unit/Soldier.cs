using System;
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

    public float GetSpeed(Map.MapTileType mapTileType)
    {
        switch (mapTileType)
        {
            case Map.MapTileType.Forest:
                return 0.1f;
            case Map.MapTileType.Grass:
                return 0.50f;
            case Map.MapTileType.Road:
                return 0.75f;
            default:
                throw new Exception("Bad tile type");
        }
    }
}
