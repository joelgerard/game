using System;
public class MovingState : BaseUnitState, IState
{
    public MovingState()
    {

    }

    public override Transition GetTransition(IState state)
    {
        if (state is AttackState)
        {
            return new Transition(new AttackState(),State.StateType.TEMPORARY);
        }
        // TODO: Return null?
        return null;
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        // TODO: Should we use the navigator to move in here?
        //throw new NotImplementedException();
        return null;
    }
}
