using System;
public class DyingState : BaseUnitState, IState
{
    public DyingState()
    {

    }

    public UnitEvent GetAssociatedEvent() 
    { 
        UnitGameEvents.UnitDyingEvent ude = new UnitGameEvents.UnitDyingEvent();
        return ude; 
        }
}
