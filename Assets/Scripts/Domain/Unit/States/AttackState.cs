using System;
using static State;

public class AttackState : BaseUnitState, IState
{

    public Transition Update(IState state)
    {
        // TODO: Maybe it should attack the other object?
        return null;
    }

    public Transition Exit()
    {
        return new Transition(this, StateType.TEMPORARY, TransitionType.EXIT);
    }

    public override Transition GetTransition(IState state)
    {
        Transition transition = base.GetTransition(state);
        return transition;
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        Transition transition = base.Update(unit, deltaTime);
        if (transition != null)
        {
            return transition;
        }
        else
        {
            bool? alive = unit.Enemy?.Damage(1.0f * deltaTime);
            if (alive != null && alive == false)
            {
                return Exit();
            }
            return null;
        }
    }
}
