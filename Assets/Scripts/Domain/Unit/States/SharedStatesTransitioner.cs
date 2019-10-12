using System;
using static State;

public class SharedStatesTransitioner
{
    public SharedStatesTransitioner()
    {
    }

    public Transition GetTransition(IState desiredState, IState currentState)
    {
        if (!(currentState is AttackState) && desiredState is AttackState)
        {
            return new Transition(new AttackState(currentState.GetUnit()), State.StateType.TEMPORARY);
        }


        // FIXME: Can everything move to the tracking state? What about bases?
       if (!(currentState is AttackState) && desiredState is TrackingState)
        {
            return new Transition(new TrackingState(currentState.GetUnit()), StateType.TEMPORARY);
        }

        return null;
    }

    public Transition Update(IState currentState, float deltaTime)
    {
        Unit unit = currentState.GetUnit();
        if (unit.HP <= 0 && !(currentState is DyingState || currentState is DeadState))
        {
            return new Transition(new DyingState(currentState.GetUnit()), StateType.NORMAL);
        }

        return null;
    }
}
