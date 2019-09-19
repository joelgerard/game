using System;
public class BaseUnitState : State
{
    public BaseUnitState()
    {
    }

    public override Transition GetTransition(State state)
    {
        if (state is AttackState)
        {
            return new Transition(new AttackState(), State.TransitionType.TEMPORARY);
        }
        return null;
    }

    public override Transition Update(Unit unit)
    {
        if (unit.HP <= 0)
        {
            return new Transition(new DeadState(),TransitionType.NORMAL);
        }
        return null;
    }
}
