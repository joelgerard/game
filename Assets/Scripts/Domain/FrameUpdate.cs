using System;
using System.Collections.Generic;

public class FrameUpdate
{
    public FrameUpdate()
    {
    }

    //public void AddCreatedEvent(Unit unit)
    //{
    //    CreatedEvents.Add(new UnitCreatedEvent(unit));
    //}

    //public List<UnitCreatedEvent> CreatedEvents { get; set; } = new List<UnitCreatedEvent>();

    public List<UnitEvent> UnitEvents { get; set; } = new List<UnitEvent>();


}
