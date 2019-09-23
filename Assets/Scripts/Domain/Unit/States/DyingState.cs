using System;
using static State;

public class DyingState : IState
{
    private DyingState()
    {

    }

    readonly Unit unit;

    public DyingState(Unit unit)
    {
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public UnitEvent GetAssociatedEvent()
    {
        UnitGameEvents.UnitDyingEvent ude = new UnitGameEvents.UnitDyingEvent
        {
            Unit = GetUnit()
        };
        return ude;
    }

    public Transition GetTransition(IState state)
    {
        if (state is DeadState)
        {
            return new Transition(new DeadState(GetUnit()), StateType.NORMAL);
        }
        return null;
    }

    public Transition Update(Unit unit, float deltaTime)
    {
        return null;
    }
}
