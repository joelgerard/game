using System;
public class UnitsCollideEvent : GameEvent
{
    public Unit Unit { get; set; }
    public Unit OtherUnit { get; set; }

    public UnitsCollideEvent(Unit unit, Unit otherUnit)
    {
        Unit = unit;
        OtherUnit = otherUnit;
    }

}
