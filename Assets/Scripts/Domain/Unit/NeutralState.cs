using System;
public class NeutralState : State
{

    // TODO: Should the parameter here be a state or input?
    public Transition GetTransition(MovingState state)
    {
        return new Transition(state, TransitionType.NORMAL);
    }

    public Transition GetTransition(AttackState state)
    {
        return new Transition(state, TransitionType.TEMPORARY);
    }

    public override Transition Update(Unit unit)
    {
        return null;
        //throw new NotImplementedException();
    }

    public override Transition GetTransition(State state)
    {
        if (state is MovingState)
        {

            return new State.Transition(new MovingState(),TransitionType.NORMAL);
        }
        return null;
        }}
        
