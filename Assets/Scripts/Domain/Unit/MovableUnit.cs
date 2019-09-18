using System;
using Hsm;
using UnityEngine;
using static MovableUnit.MovableUnitHsm;

public partial class MovableUnit : Unit
{
    protected bool startMoving = false;

    public partial class MovableUnitHsm { }
    public float Speed { get; set; } = 1.5f;
    public Vector2 TargetPosition {get;set;}
    public int IndexOnPath { get; set; } = 0;

    public MovableUnit()
    {
    }

    public void StartMoving()
    {
        // TODO: In the neutral state, you can start moving.
        // TODO: There should also be a universal state transition.
        // That is, you can always start moving, or attacking from any
        // state. Update state with the move transition. What should it call?
        // Call the neutral updatestate function. ???????

        //GameController.Log("Start moving");
        if (!startMoving)
        {
            IndexOnPath = 0;
        }
        startMoving = true;

        StateMachine.Update(new MovingState());

    }

    /** ############ SHARED STATES ########### */
    // C# HSM doesn't yet support shared states.
    protected override Transition GetNeutralTransition()
    {
        return Transition.Sibling<MovableNeutral>();
    }
}
