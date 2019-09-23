using System;
public class UnitGameEvents
{
    public class UnitDyingEvent : UnitEvent { }
    public class UnitDiedEvent : UnitEvent { }
    public class UnitCreatedEvent : UnitEvent { }

    public UnitGameEvents()
    {
    }
}
