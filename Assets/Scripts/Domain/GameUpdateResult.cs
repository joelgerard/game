using System;
using System.Collections.Generic;

public class GameUpdateResult
{
    public GameUpdateResult()
    {
    }

    //public void AddCreatedEvent(Unit unit)
    //{
    //    CreatedEvents.Add(new UnitCreatedEvent(unit));
    //}

    //public List<UnitCreatedEvent> CreatedEvents { get; set; } = new List<UnitCreatedEvent>();

    public List<UnitEvent> UnitEvents { get; set; } = new List<UnitEvent>();


}
