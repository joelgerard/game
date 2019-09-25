using System;
using System.Collections.Generic;
using UnityEngine;

public enum Allegiance
{
    ALLY,
    ENEMY
}

public partial class Unit
{
    private Vector2 position = new Vector2();


    public Unit Enemy { get; set; } = null;

    // TODO: Think about this.
    public GameObject GameObject { set; get; }

    public string Name { get; set; }

    public Unit()
    {
        StateMachine = new StateMachine(this);
    }

    public Allegiance Allegiance {get;set;}
    public float HP { get; set; }
    public float TotalHP { get; set; }
    public StateMachine StateMachine { get; set; }


    public virtual void Init()
    {
        TotalHP = HP = 10;
    }

    public float PercentHealth
    {
        get
        {
            return HP / TotalHP;
        }
    }

    public Vector2 Position
    {
        get
        {
            // TODO: Think about this.
            if (this.GameObject == null)
            {
                return position;
            }
            else 
            { 
                return this.GameObject.transform.position;
            }
        }
        set
        {
            if (this.GameObject == null)
            {
                position = value;
            }
            else
            { 
                this.GameObject.transform.position = value;
            }
        }
    }

    public virtual bool Damage(Unit enemyUnit, float damage)
    {
        HP -= damage;

        // Attack back if not in attack state and somebody is
        // attacking this unit. 
        if (!StateMachine.IsInState<AttackState>())
        {
            this.Enemy = enemyUnit;
            this.StateMachine.Transition(new AttackState(this));
        }

        return HP > 0;
    }

    public virtual void Attack(Unit otherUnit)
    {

        if (otherUnit.Allegiance != this.Allegiance)
        {
            Enemy = otherUnit;
            StateMachine.Transition(new AttackState(this));
        }

    }



    public virtual UnitEvent Update(float deltaTime)
    {
        //oldStateMachine.Update(deltaTime);

        // TODO: Something is messed up here.
        // Statemachine returns something???
        // I Guess if there is a new state, then it can pass back to the renderer
        State.Transition transition = StateMachine.Update(this, deltaTime);
        if (transition != null)
        {
            UnitEvent ue = transition.State.GetAssociatedEvent();
            if (ue != null)
            { 
                ue.Unit = this;
                return ue;
            }
        }
        return null;
    }
}
