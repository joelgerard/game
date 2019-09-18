using System;
using System.Collections.Generic;
using Hsm;
using UnityEngine;
using static Unit.UnitHsm;

public enum Allegiance
{
    ALLY,
    ENEMY
}

public partial class Unit
{
    // TODO: no public
    public Hsm.StateMachine oldStateMachine = new Hsm.StateMachine();
    public partial class UnitHsm { }

    private Vector2 position = new Vector2();

    protected bool startAttack = false;
    protected Unit enemy = null;

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
        oldStateMachine.Init<UnitHsm.Root>(this);
        oldStateMachine.TraceLevel = Hsm.TraceLevel.Diagnostic;
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
            OnDestroyedEvent(this);
        }
        else
        {
            OnDamagedEvent?.Invoke(this, PercentHealth);
        }
        return HP > 0;
    }

    public virtual void Attack(Unit otherUnit)
    {
        if (otherUnit.Allegiance != this.Allegiance)
        {
            // TODO: Integrate HSM further. Better? Where should this "if" be? 
            startAttack = true;
            enemy = otherUnit;
            StateMachine.Update(new AttackState());
            otherUnit.Defend(this);
        }

    }

    public virtual void Defend(Unit attackingUnit)
    {
        // TODO: write this
    }

    public virtual void Update(float deltaTime)
    {
        //oldStateMachine.Update(deltaTime);
        if (StateMachine.IsInState<AttackState>())
        {
            // TODO: Should this really be here? Shouldn't this be in 
            // Attack state update?
            enemy?.Damage(1.0f * deltaTime);
        }
    }

    /** ############ SHARED STATES ########### */
    // C# HSM doesn't yet support shared states.
    protected virtual Transition GetNeutralTransition()
    {
        return Transition.Sibling<Neutral>();
    }

}
