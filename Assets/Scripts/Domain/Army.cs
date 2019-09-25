using System;
using System.Collections.Generic;
using static UnitGameEvents;

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


    public List<UnitEvent> Update(float deltaTime, IPath path)
    {
        List<UnitEvent> unitEvents = new List<UnitEvent>();
        // TODO: Must be a better way than this.
        foreach (Soldier soldier in Soldiers)
        {
            UnitEvent ue = soldier.Update(deltaTime);
            if (ue != null)
            {
                unitEvents.Add(ue);
            }
        }

        UnitEvent armyBaseEvent = ArmyBase.Update(deltaTime);
        if (armyBaseEvent is UnitDyingEvent && ArmyBase.Allegiance == Allegiance.ENEMY)
        {
            GameController.Log("YOU WIN");
        }
        if (armyBaseEvent is UnitDyingEvent && ArmyBase.Allegiance == Allegiance.ALLY)
        {
            GameController.Log("YOU LOSE");
        }


        navigator.MoveUnits(deltaTime, Soldiers, path);
        return unitEvents;
    }

    public TerritoryType Territory { get; set; } = TerritoryType.South;

    // TODO: Returns Soldier?
    public void AddSoldier(Soldier soldier)
    {
        Soldiers.Add(soldier);
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
