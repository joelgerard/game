using System;
public class UnitEvent
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
