using System;
using UnityEngine;

public enum Allegiance
{
    ALLY,
    ENEMY
}

public class Unit
{


    public Unit()
    {
    }

    public Allegiance Allegiance {get;set;}
    public int HP { get; set; }

    public void Attack(Unit otherUnit)
    {
        if (otherUnit.Allegiance != this.Allegiance)
        { 
            Debug.Log("Unit will attack");
        }
    }
}
