using System;
using static State;

public class NeutralState : IState
{
    private NeutralState()
    {

    }

    readonly Unit unit;

    public NeutralState(Unit unit)
    {
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    // TODO: Should the parameter here be a state or input?
    public Transition GetTransition(MovingState state)
    {
        return new Transition(state, StateType.NORMAL);
    }

    public Transition GetTransition(AttackState state)
    {
        return new Transition(state, StateType.TEMPORARY);
    }

    public  Transition Update(Unit unit, float deltaTime)
    {
        return null;
        //throw new NotImplementedException();
    }

    public  Transition GetTransition(IState state)
    {
        // TODO: This feels horrible man. 
        // TODO: It is. Also, can just dispatch from here.
        if (state is AttackState)
        {
            return new State.Transition(new AttackState(GetUnit()), StateType.TEMPORARY);
        }
        if (state is MovingState)
        {
            return new State.Transition(new MovingState(GetUnit()),StateType.NORMAL);
        }
        throw new ArgumentException("Cannot transition to the requested state. " + state.ToString(), "state");
    }

    public UnitEvent GetAssociatedEvent() { return null; }
}
        
