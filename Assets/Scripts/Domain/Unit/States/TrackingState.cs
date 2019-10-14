using System;
using static State;

public class TrackingState : MovingState
{
    public TrackingState(Unit unit) : base(unit)
    {

    }

    public Transition Exit()
    {
        unit.Enemy = null;
        return new Transition(this, StateType.TEMPORARY, TransitionType.EXIT);
    }

    public override Transition GetTransition(IState state)
    {
        if (state is AttackState)
        {
            return new Transition(new AttackState(state.GetUnit()), State.StateType.NORMAL);
        }
        return null;
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        Transition transition = base.Update(unit, deltaTime);

        if (transition!= null && unit.Enemy != null && unit.Enemy.IsDeadOrDying())
        {
            return Exit();
        }

        return null;
    }
}
