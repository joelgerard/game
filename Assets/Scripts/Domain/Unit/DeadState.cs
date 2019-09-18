using System;
public class DeadState : State
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
