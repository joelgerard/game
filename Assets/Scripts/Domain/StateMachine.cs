using System;
using System.Collections.Generic;

public class StateMachine
{
    public Stack<IState> States { get; } = new Stack<IState>();
    Unit unit;

    public StateMachine(Unit unit)
    {
        this.unit = unit;
    }

    private StateMachine()
    {
    }

    public IState CurrentState
    {
        get
        {
            if (States.Count < 1)
            {
                // TODO: Think about this. 
                return new NeutralState(this.unit);
            }
            return States.Peek();
        }
    }

    void SetState(IState state)
    {
        if (States.Count > 0)
        {
            States.Pop();
        }
        PushState(state);
    }

    void PushState(IState state)
    {
        States.Push(state);
    }

    void ResumePrevState()
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
        GameController.Log(unit?.Name + " is seeking transition from " + CurrentState.GetType() + " to state " + desiredState.GetType());
        State.Transition transition = CurrentState.GetTransition(desiredState);

        // TODO: DRY with update fn
        if (transition != null)
        {
            GameController.Log("Granted transition for " + unit?.Name);
            if (transition.StateType == State.StateType.NORMAL)
            {
                GameController.Log("Setting state to normal " + transition.State.GetType());
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

    public State.Transition Update(Unit unit, float deltaTime)
    {
        State.Transition transition = CurrentState.Update(unit, deltaTime);
        if (transition != null)
        {
            GameController.Log("Unit " + unit.Name + " seeking state " + transition.State.GetType().ToString());
            IState nextState = transition.State;

            if (transition.TransitionType == State.TransitionType.ENTER)
            {
                if (nextState.GetType().Equals(CurrentState.GetType()))
                {
                    throw new Exception("Unit " + unit.Name + " cannot transition from this state to the same state: " + CurrentState.GetType().ToString());
                }
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
            return transition;
        }
        else
        {
            // TODO: Null?
            return null;
        }
    }


}
