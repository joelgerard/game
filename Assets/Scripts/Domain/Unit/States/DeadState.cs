using System;
public class DeadState : BaseUnitState
{
    public DeadState()
    {
    }

    public override Transition GetTransition(State state)
    {
        return null;
    }

    public override Transition Update(Unit unit)
    {
        return null;
    }
}
