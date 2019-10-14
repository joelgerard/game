using System;
using System.Collections.Generic;
using static GameEvents;
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


    public List<GameEvent> Update(float deltaTime, IPath path)
    {
        List<GameEvent> gameEvents = new List<GameEvent>();
        // TODO: Must be a better way than this.
        foreach (Soldier soldier in Soldiers)
        {
            UnitEvent ue = soldier.Update(deltaTime);
            if (ue != null)
            {
                gameEvents.Add(ue);
            }
        }

        UnitEvent armyBaseEvent = ArmyBase.Update(deltaTime);
        if (armyBaseEvent is UnitDyingEvent && ArmyBase.Allegiance == Allegiance.ENEMY)
        {
            GameOverEvent gameOverEvent = new GameOverEvent
            {
                isPlayerWinner = true
            };
            gameEvents.Add(gameOverEvent);
        }
        if (armyBaseEvent is UnitDyingEvent && ArmyBase.Allegiance == Allegiance.ALLY)
        {
            GameOverEvent gameOverEvent = new GameOverEvent
            {
                isPlayerWinner = false
            };
            gameEvents.Add(gameOverEvent);
        }

        //navigator.MoveUnits(deltaTime, Soldiers, path);
        return gameEvents;
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
