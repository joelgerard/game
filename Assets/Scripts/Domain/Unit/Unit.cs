﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum Allegiance
{
    ALLY,
    ENEMY
}

public partial class Unit
{
    // TODO: no public
    //public Hsm.StateMachine oldStateMachine = new Hsm.StateMachine();
    //public partial class UnitHsm { }

    private Vector2 position = new Vector2();


    public Unit Enemy { get; set; } = null;

    public event OnDestroyed OnDestroyedEvent;
    public delegate void OnDestroyed(Unit unitDestroyed);

    public event OnDamaged OnDamagedEvent;
    public delegate void OnDamaged(Unit unitDamaged, float percentHealth);

    // TODO: Think about this.
    public GameObject GameObject { set; get; }

    public string Name { get; set; }

    public Unit()
    {
    }

    public Allegiance Allegiance {get;set;}
    public float HP { get; set; }
    public float TotalHP { get; set; }
    public StateMachine StateMachine { get; set; } = new StateMachine();


    public virtual void Init()
    {
        // TODO: Init new state machine?

        //oldStateMachine.Init<UnitHsm.Root>(this);
        //oldStateMachine.TraceLevel = Hsm.TraceLevel.Diagnostic;
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

    public virtual bool Damage(float damage)
    {
        HP -= damage;
        if (HP <= 0 && OnDestroyedEvent != null)
        {
            // TODO: Get rid of all these events like this.
            //OnDestroyedEvent(this);
        }
        else
        {
            OnDamagedEvent?.Invoke(this, PercentHealth);
        }
        if (HP <=0)
        {
            GameController.Log("Unit " + this.Name + " destroyed");
        }
        return HP > 0;
    }

    public virtual void Attack(Unit otherUnit)
    {

        if (otherUnit.Allegiance != this.Allegiance)
        {
            Enemy = otherUnit;
            StateMachine.Transition(new AttackState());
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

    /** ############ SHARED STATES ########### */
    // C# HSM doesn't yet support shared states.
    //protected virtual Transition GetNeutralTransition()
    //{
    //    return Transition.Sibling<Neutral>();
    //}

}
