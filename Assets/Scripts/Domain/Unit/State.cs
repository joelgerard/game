using System;
public abstract class State
{

    public enum TransitionType
    {
        NORMAL,
        TEMPORARY
    }

    public class Transition
    {
        public Transition(State state, TransitionType transitionType)
        {
            this.State = state;
            this.TransitionType = transitionType;
        }

        public TransitionType TransitionType { get; set; } = TransitionType.NORMAL;
        public State State { get; set; }
    }

    public abstract Transition GetTransition(State state);


}
