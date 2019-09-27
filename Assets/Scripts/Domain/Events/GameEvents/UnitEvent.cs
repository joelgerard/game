using System;
public class UnitEvent : GameEvent
{
    public Unit Unit { get; set; }
    public UnitEvent()
    {
    }

    public UnitEvent(Unit unit)
    {
        Unit = unit;
    }
}
