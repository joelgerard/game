using System;
using static State;

public class AttackState : IState
{
    readonly Unit unit;

    public AttackState(Unit unit)
    {
        this.unit = unit;
    }

    private AttackState()
    {

    }

    public Transition Update(IState state)
    {
        // TODO: Maybe it should attack the other object?
        return null;
    }

    public Transition Exit()
    {
        return new Transition(this, StateType.TEMPORARY, TransitionType.EXIT);
    }

    public  Transition GetTransition(IState state)
    {
        return (new SharedStatesTransitioner()).GetTransition(state,this);
    }

    public  Transition Update(Unit unit, float deltaTime)
    {
        Transition transition = (new SharedStatesTransitioner()).Update(this, deltaTime);
        if (transition != null)
        {
            return transition;
        }
        else
        {
            bool? alive = unit.Enemy?.Damage(unit, 1.0f * deltaTime);
            if (alive != null && alive == false)
            {
                return Exit();
            }
            return null;
        }
    }

    public UnitEvent GetAssociatedEvent() { return null; }

    public Unit GetUnit()
    {
        return unit;
    }
}
