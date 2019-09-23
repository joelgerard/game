using System;
public class UnitExplosionComplete : UnityGameEvent
{
    public string UnitName { get; set; }
    public UnitExplosionComplete()
    {
    }

    public UnitExplosionComplete(string unitName)
    {
        UnitName = unitName;
    }
}
