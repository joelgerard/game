using System;
public class AttackState : State
{

    public Transition Update(State state)
    {
        // TODO: Only state to go here is death.
        return null;
    }

    public Transition Exit()
    {
        return new Transition(this, TransitionType.TEMPORARY);
    }

    public override Transition GetTransition(State state)
    {
        throw new NotImplementedException();
    }
}
