using System;
public class UnitState : State
{
    public UnitState()
    {
    }

    public override Transition GetTransition(State state)
    {
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
