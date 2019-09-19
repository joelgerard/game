using System;
public abstract class State
{

    public enum StateType
    {
        NORMAL,
        TEMPORARY
    }

    public enum TransitionType
    {
        ENTER,
        EXIT
    }

    public class Transition
    {
        public Transition(IState state, StateType stateType) : this(state, stateType, TransitionType.ENTER)
        {

        }

        public Transition(IState state, StateType stateType, TransitionType transitionType)
        {
            this.State = state;
            this.StateType = stateType;
            this.TransitionType = transitionType;
        }

        public StateType StateType { get; set; } = StateType.NORMAL;
        public TransitionType TransitionType { get; set; }
        public IState State { get; set; }
    }

    public abstract Transition Update(Unit unit, float deltaTime);
    public abstract Transition GetTransition(IState state);


}
