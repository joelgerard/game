using System;
public class ArmyBase : Unit
{
    public ArmyBase()
    {
    }

    public override void Init()
    {
        base.Init();
        TotalHP = HP = 100;
    }
}
