using System;
public class UnitsCollideEvent : UnityGameEvent
{
    public enum CollisionEventType
    {
        ATTACK,
        SIGHT
    }

    public String Unit { get; set; }
    public String OtherUnit { get; set; }
    public CollisionEventType? CollisionType { get; set; } = null;

    // TODO: Why does this take strings?
    public UnitsCollideEvent(String unit, String otherUnit, CollisionEventType collisionEventType)
    {
        Unit = unit;
        OtherUnit = otherUnit;
        CollisionType = collisionEventType;
    }

}
