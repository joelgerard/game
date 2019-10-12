using System;
using static State;

public class TrackingState : MovingState
{
    public TrackingState(Unit unit) : base(unit)
    {

    }

    public Transition Exit()
    {
        unit.Enemy = null;
        return new Transition(this, StateType.TEMPORARY, TransitionType.EXIT);
    }

    public override Transition GetTransition(IState state)
    {
        if (state is AttackState)
        {
            return new Transition(new AttackState(state.GetUnit()), State.StateType.NORMAL);
        }
        return null;
    }
}
