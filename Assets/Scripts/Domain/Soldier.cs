using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Soldier : Unit
{
    GameObject go;

    public partial class SoliderHsm { }
    private Hsm.StateMachine mStateMachine = new Hsm.StateMachine();

    private Hsm.StateValue<int> StateValue_HP = new Hsm.StateValue<int>(100);

    private bool startAttack = false;

    public Soldier(GameObject go)
    {
        this.go = go;

    }

    public void Init()
    {
        Debug.Log("Set soldier to root state");
        mStateMachine.Init<SoldierHsm.Root>(this);
        mStateMachine.TraceLevel = Hsm.TraceLevel.Diagnostic;
    }

    public override void Update(float deltaTime)
    {
        mStateMachine.Update(deltaTime);
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

    public override void Attack(Unit otherUnit)
    {
        if (otherUnit.Allegiance != this.Allegiance)
        {
            // TODO: Integrate HSM further. 
            Debug.Log("Unit will attack in soldier");
            startAttack = true;

        }
    }

    public float SpeedWeight { get; }
}
