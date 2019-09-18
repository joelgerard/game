using System;
public class MovingState : State
{
    public MovingState()
    {

    }

    public override Transition GetTransition(State state)
    {
        if (state is AttackState)
        {
            return new State.Transition(new AttackState(),State.TransitionType.TEMPORARY);
        }
        // TODO: Return null?
        return null;
    }
}
