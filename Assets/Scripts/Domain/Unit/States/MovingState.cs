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
        Transition trans = base.Update(unit, deltaTime);
        if (trans != null)
        {
            return trans;
        }

        // TODO: Should we use the navigator to move in here?
        // Makes it hard I think. Think about it. Resolve the return trans
        // if the answer is no, i.e. can just return base. 
        //throw new NotImplementedException();
        return null;
    }

    public UnitEvent GetAssociatedEvent() { return null; }
}
