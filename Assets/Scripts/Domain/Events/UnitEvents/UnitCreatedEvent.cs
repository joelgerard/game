using System;
public class UnitCreatedEvent : UnitEvent
{
    public UnitCreatedEvent()
    {
    }

    public UnitCreatedEvent(Unit unit) : base(unit)
    {
    }
}
