using System;
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
    public Hsm.StateMachine StateMachine = new Hsm.StateMachine();
    public partial class UnitHsm { }

    protected bool startAttack = false;
    protected Unit enemy = null;

    public event OnDestroyed OnDestroyedEvent;
    public delegate void OnDestroyed(Unit unitDestroyed);

    protected GameObject go;

    public Unit()
    {
    }

    public Allegiance Allegiance {get;set;}
    public int HP { get; set; }

    public virtual void Init()
    {
        StateMachine.Init<UnitHsm.Root>(this);
        StateMachine.TraceLevel = Hsm.TraceLevel.Diagnostic;
        HP = 10;
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

    public virtual bool Damage(int damage)
    {
        HP -= damage;
        if (HP < 1 && OnDestroyedEvent != null)
        {
            OnDestroyedEvent(this);
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
        }
    }

    public virtual void Update(float deltaTime)
    {
        StateMachine.Update(deltaTime);
    }

    /** ############ SHARED STATES ########### */
    // C# HSM doesn't yet support shared states.
    protected virtual Transition GetNeutralTransition()
    {
        return Transition.Sibling<Neutral>();
    }

}
