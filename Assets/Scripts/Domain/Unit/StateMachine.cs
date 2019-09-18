using System;
using System.Collections.Generic;

public class StateMachine
{
    State currentState;
    public Stack<State> States { get; } = new Stack<State>();

    public State CurrentState
    {
        get
        {
            if (States.Count < 1)
            {
                // TODO: Think about this. 
                return new NeutralState();
            }
            return States.Peek();
        }
    }

    public void SetState(State state)
    {
        if (States.Count > 0)
        {
            States.Pop();
        }
        PushState(state);
    }

    public void PushState(State state)
    {
        States.Push(state);
    }

    public event OnStateChange OnStateChangeEvent;
    public delegate void OnStateChange(Unit unitDestroyed);

    public StateMachine()
    {
    }

    public bool IsInState<T>()
    {
        if (States.Count > 0)
        {
            return CurrentState is T;
        }
        else
        {
            return false;
        }
    }

    public State Update(State state)
    {
        // TODO: Transition has a type inside.
        State.Transition transition = CurrentState.GetTransition(state);
        if (transition != null)
        {
            if (transition.TransitionType == State.TransitionType.NORMAL)
            {
                SetState(transition.State);
            }
            else if (transition.TransitionType == State.TransitionType.TEMPORARY)
            {
                PushState(transition.State);
            }
            return transition.State;
        }
        else
        {
            // TODO: Null?
            return null;
        }
    }


}
