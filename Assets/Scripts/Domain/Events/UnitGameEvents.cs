using System;
public class UnitGameEvents
{
    Unit Unit { get; set; }

    public class UnitDyingEvent : UnitEvent { }

    public UnitGameEvents()
    {
    }
}
