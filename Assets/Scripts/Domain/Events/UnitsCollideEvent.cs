using System;
public class UnitsCollideEvent : GameEvent
{
    public String Unit { get; set; }
    public String OtherUnit { get; set; }

    // TODO: Why does this take strings?
    public UnitsCollideEvent(String unit, String otherUnit)
    {
        Unit = unit;
        OtherUnit = otherUnit;
    }

}
