using System;
using System.Collections.Generic;
using UnityEngine;

// This is the game as if it were a board game, i.e. no unity runtime stuff here.
// Just logic. Should be able to instantiate and test this anywhere. 
public class Game
{
    public List<Army> Armies { get; set; } = new List<Army>();
    public Army Player { get { return Armies[1]; } }
    public Army Enemy { get { return Armies[0]; } }
    public AI AI { get; set; } = new AI();

    // TODO: This path is hard coded for now.
    public Path Path { get; set; } = new Path();

    // TODO: I guess string comp is expensive. 
    // TODO: Why is this here? 
    public Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();

    public Game()
    {
    }

    public void Initialize()
    {

        //Enemy
        AddArmy();

        // Player
        AddArmy(); //TODO: Not yet used.
  }

    public List<Unit> Update(GameUpdate update)
    {
        List<Unit> createdUnits = new List<Unit>();
        foreach(GameEvent curEvent in update.GameEvents)
        {
            // TODO: consider moving to different function.
            if (curEvent is HomeBaseClickEvent)
            {
                createdUnits.Add(HomeBaseClickedEvent(curEvent as HomeBaseClickEvent));
            }
        }
        // TODO: Need to call this once per frame?
        Player.Update(update.deltaTime, update.currentPath);
        Enemy.Update(update.deltaTime, Path);

        return createdUnits;
    }

    // Used to control game logic like army growth etc.
    //public List<TurnUpdate> TurnUpdate()
    public List<Unit> TurnUpdate()
    {
        List<Unit> createdUnits = new List<Unit>();
        createdUnits.AddRange(AI.Update(this, Player, Enemy));
        return createdUnits;
    }

    // TODO: Public?
    public List<Unit> DrawMap()
    {
        // Enemy
        Enemy.ArmyBase = CreateBase(Allegiance.ENEMY, new Vector2(0f, 3f));
        Enemy.ArmyBase.Name = "EnemyBaseSquare";
        Enemy.ArmyBase.OnDestroyedEvent += EnemyBase_OnDestroyedEvent;

        // Enemy
        Player.ArmyBase = CreateBase(Allegiance.ALLY, new Vector2(0f, -3f));
        Player.ArmyBase.Name = "PlayerBaseSquare";
        //Player.ArmyBase.OnDestroyedEvent += EnemyBase_OnDestroyedEvent;

        return new List<Unit>
        {
            Enemy.ArmyBase
            ,Player.ArmyBase
        };
    }

    private ArmyBase CreateBase(Allegiance allegiance, Vector2 pos)
    {
        ArmyBase unitBase = new ArmyBase
        {
            Allegiance = allegiance,
            Position = pos
        };
        unitBase.Init();

        return unitBase;
    }

    private Army GetSoldierArmy(Unit soldier)
    {
        return (soldier.Allegiance == Allegiance.ALLY ? Player : Enemy);
    }

    private Unit HomeBaseClickedEvent(HomeBaseClickEvent e)
    {
        return AddSoldier(Allegiance.ALLY, e.pos);
    }

    public void OnUnitRenderedEvent(Unit unit)
    {
        this.unitMap.Add(unit.GameObject.name, unit);
    }

    public Army AddArmy()
    {
        Army army = new Army();
        Armies.Add(army);
        return army;
    }

    public void OnPathReady()
    {
        Player.StartMoving();
    }

    public Soldier AddSoldier(Allegiance allegiance, Vector2 pos)
    {
        Soldier soldier = new Soldier()
        {
            Allegiance = allegiance,
            Position = pos

        };

        List<Soldier> soldiers = (allegiance == Allegiance.ALLY ? Player.Soldiers : Enemy.Soldiers);
        soldier.Init();
        soldier.OnDestroyedEvent += Soldier_OnDestroyedEvent;
        soldiers.Add(soldier);
        return soldier;
    }

    //public Soldier OnAddSoldier(GameObject gameObject, Allegiance allegiance)
    //{
    //    Soldier soldier = new Soldier()
    //    {
    //        Allegiance = allegiance,
    //        GameObject = gameObject
    //    };

    //    // TODO: If statement here is bad. 
    //    List<Soldier> soldiers = (allegiance == Allegiance.ALLY ? Player.Soldiers : Enemy.Soldiers);
    //    soldier.Init();
    //    soldier.OnDestroyedEvent+= Soldier_OnDestroyedEvent;
    //    soldiers.Add(soldier);
    //    unitMap.Add(gameObject.name, soldier);
    //    return soldier;
    //}

    void EnemyBase_OnDestroyedEvent(Unit destroyedUnit)
    {
        // TODO: This can be cleaned up DRY. 
        //unitMap.Remove(destroyedUnit.GameObject.name);
        unitMap.Remove(destroyedUnit.Name);

        GameController.Log("You win.");
    }

    void Soldier_OnDestroyedEvent(Unit unitDestroyed)
    {
        // TODO: Feels like all the units and gameobjects can be managed at once? 
        // FIXME: What is happening to all the gameobjects? Is this a bug?
        // Whatever it is, it has a null reference bug. 

        // FIXME: This is only looking up player soldiers. What happens when the
        // enemy soldier dies?
        Army army = GetSoldierArmy(unitDestroyed);
        // TODO: Why does this have to search by name when it already has the object?
        Soldier soldier = army.Soldiers.Find((Soldier obj) => obj.GameObject.name == unitDestroyed.GameObject.name);
        unitMap.Remove(unitDestroyed.GameObject.name);
        army.Soldiers.Remove(soldier);
    }


    // TODO: Why does this take strings?
    public void OnUnitsCollide(String unit1Name, String unit2Name)
    {
        Diagnostics.KeyExists(unitMap, unit1Name);
        Diagnostics.KeyExists(unitMap, unit2Name);

        Unit unit = unitMap[unit1Name];
        Unit otherUnit = unitMap[unit2Name];
        unit.Attack(otherUnit);
    }
}
