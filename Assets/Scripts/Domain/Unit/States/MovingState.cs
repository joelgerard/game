using System;
public class MovingState : BaseUnitState
{
    public MovingState()
    {

    }

    public override Transition GetTransition(State state)
    {
        if (state is AttackState)
        {
            return new Transition(new AttackState(),State.TransitionType.TEMPORARY);
        }
        // TODO: Return null?
        return null;
    }

    public override Transition Update(Unit unit)
    {
        // TODO: Should we use the navigator to move in here?
        //throw new NotImplementedException();
        return null;
    }
}
