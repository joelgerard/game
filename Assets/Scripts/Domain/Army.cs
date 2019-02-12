using System;
using System.Collections.Generic;

public class Army
{
    public enum TerritoryType { North, South};
    private Dictionary<TerritoryType, int> attackDirections;

    List<Soldier> Soldiers { get; set; } = new List<Soldier>();

    public Army()
    {
        attackDirections = new Dictionary<TerritoryType, int> {
            {TerritoryType.North, -1},
            {TerritoryType.South, 1}
        };

    }

    public TerritoryType Territory { get; set; } = TerritoryType.South;

    public Soldier AddSoldier()
    {
        Soldier soldier = new Soldier();
        Soldiers.Add(soldier);
        return soldier;
    }

    public int AttackDirection { get { return attackDirections[Territory]; } }

}
