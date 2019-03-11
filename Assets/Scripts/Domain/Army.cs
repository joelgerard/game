using System;
using System.Collections.Generic;

public class Army
{
    public enum TerritoryType { North, South};
    private Dictionary<TerritoryType, int> attackDirections;
    Navigator navigator = new Navigator();
    public ArmyBase ArmyBase { get; set; }

    public List<Soldier> Soldiers { get; set; } = new List<Soldier>();

    public Army()
    {
        attackDirections = new Dictionary<TerritoryType, int> {
            {TerritoryType.North, -1},
            {TerritoryType.South, 1}
        };

    }


    public void Update(float deltaTime, IPath path)
    {

        // TODO: Must be a better way than this.
        foreach (Soldier soldier in Soldiers)
        {
            soldier.Update(deltaTime);
        }
        navigator.MoveUnits(deltaTime, Soldiers, path);
    }

    public TerritoryType Territory { get; set; } = TerritoryType.South;

    public Soldier AddSoldier(Soldier soldier)
    {
        Soldiers.Add(soldier);

        /*Soldier soldier = new Soldier();
        Soldiers.Add(soldier);
        return soldier;*/
        return null;
    }

    public void StartMoving()
    {
        foreach (Soldier soldier in Soldiers)
        {
            soldier.StartMoving();
        }
    }

    public int AttackDirection { get { return attackDirections[Territory]; } }

}
