using System;
public class BaseUnitState : State 
{
    public BaseUnitState()
    {
    }

    public override Transition GetTransition(IState state)
    {
        if (state is AttackState)
        {
            return new Transition(new AttackState(), State.StateType.TEMPORARY);
        }
        return null;
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        if (unit.HP <= 0)
        {
            return new Transition(new DeadState(),StateType.NORMAL);
        }
        return null;
    }
}
