﻿using System;
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

    public virtual void Attack(Unit otherUnit)
    {
        if (otherUnit.Allegiance != this.Allegiance)
        {
            // TODO: Integrate HSM. 
            Debug.Log("Unit will attack");
        }
    }

    public virtual void Update(float deltaTime) { }

}