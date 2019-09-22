using System;
public class BaseUnitState : State 
{
    public BaseUnitState()
    {
    }

    public override Transition GetTransition(IState state)
    {
        // TODO: WTF is going on in here? FIXME.
        if (state is AttackState)
        {
            return new Transition(new AttackState(), State.StateType.TEMPORARY);
        }
        return null;
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        // This one makes sense and applies to all units.
        // FIXME: and test. Transitions only apply when moving states
        // This will stay in the same state and trigger a transition.
        if (!(this is DyingState) && unit.HP <= 0)
        {
            return new Transition(new DyingState(),StateType.NORMAL);
        }
        return null;
    }

    public UnitEvent AssociatedEvent { get; set; } = null;
}
