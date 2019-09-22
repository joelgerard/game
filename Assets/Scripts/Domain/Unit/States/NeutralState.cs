using System;
public class NeutralState : BaseUnitState, IState
{

    // TODO: Should the parameter here be a state or input?
    public Transition GetTransition(MovingState state)
    {
        return new Transition(state, StateType.NORMAL);
    }

    public Transition GetTransition(AttackState state)
    {
        return new Transition(state, StateType.TEMPORARY);
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        return null;
        //throw new NotImplementedException();
    }

    public override Transition GetTransition(IState state)
    {
        // TODO: This feels horrible man. 
        if (state is AttackState)
        {
            return new State.Transition(new AttackState(), StateType.TEMPORARY);
        }
        if (state is MovingState)
        {
            return new State.Transition(new MovingState(),StateType.NORMAL);
        }
        throw new ArgumentException("Cannot transition to the requested state. " + state.ToString(), "state");
    }

    public UnitEvent GetAssociatedEvent() { return null; }
}
        
