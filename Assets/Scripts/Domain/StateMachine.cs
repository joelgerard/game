using System;
using System.Collections.Generic;

public class StateMachine
{
    IState currentState;
    public Stack<IState> States { get; } = new Stack<IState>();

    public IState CurrentState
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

    public void SetState(IState state)
    {
        if (States.Count > 0)
        {
            States.Pop();
        }
        PushState(state);
    }

    public void PushState(IState state)
    {
        States.Push(state);
    }

    public event OnStateChange OnStateChangeEvent;
    public delegate void OnStateChange(Unit unitDestroyed);

    public StateMachine()
    {
    }

    public void ResumePrevState()
    {
        States.Pop();
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

    // TODO: What does first, update or transition?
    public IState Transition(IState desiredState)
    {
        State.Transition transition = CurrentState.GetTransition(desiredState);

        // TODO: DRY with update fn
        if (transition != null)
        {
            if (transition.StateType == State.StateType.NORMAL)
            {
                SetState(transition.State);
            }
            else if (transition.StateType == State.StateType.TEMPORARY)
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

    public IState Update(Unit unit, float deltaTime)
    {
        State.Transition transition = CurrentState.Update(unit, deltaTime);
        if (transition != null)
        {
            if (transition.TransitionType == State.TransitionType.ENTER)
            {
                if (transition.StateType == State.StateType.NORMAL)
                {
                    SetState(transition.State);
                }
                else if (transition.StateType == State.StateType.TEMPORARY)
                {
                    PushState(transition.State);
                }
            } else if (transition.TransitionType == State.TransitionType.EXIT)
            {
                States.Pop();
            }
            else
            {
                throw new ArgumentException("Bad transition type");
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
