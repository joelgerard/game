using System;
public class DeadState : BaseUnitState, IState
{
    public DeadState()
    {
    }

    public override Transition GetTransition(IState state)
    {
        return null;
    }

    public override Transition Update(Unit unit, float deltaTime)
    {
        return null;
    }

    public UnitEvent GetAssociatedEvent() { return null; }

}
