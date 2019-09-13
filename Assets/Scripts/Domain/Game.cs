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

    // TODO: This path is hard coded for now.
    public Path Path { get; set; } = new Path();

    // TODO: I guess string comp is expensive. 
    public Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();

    public Game()
    {
    }

    public void Initialize()
    {

        // Enemy
        AddArmy(); // TODO: This isn't doing anything.
        ArmyBase enemyBase = new ArmyBase
        {
            Allegiance = Allegiance.ENEMY
        };

        enemyBase.Init();
        unitMap.Add("EnemyBaseSquare", enemyBase);
        // TODO: Lame
        Enemy.ArmyBase = enemyBase;
        enemyBase.OnDestroyedEvent += EnemyBase_OnDestroyedEvent;

        // Player
        AddArmy(); //TODO: Not yet used.
        ArmyBase playerBase = new ArmyBase
        {
            Allegiance = Allegiance.ALLY
        };
        playerBase.Init();
        unitMap.Add("PlayerBaseSquare", playerBase);

        Path.target = playerBase.Position;
        GameController.Log("Path target is " + Path.target);
  }

    private List<Unit> DrawMap()
    {

    }



    public List<Unit> Update(GameUpdate update)
    {
        List<Unit> createdUnits = new List<Unit>();
        // TODO: No reason this is a stack really.
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

    private Unit HomeBaseClickedEvent(HomeBaseClickEvent e)
    {
        return AddSoldier(Allegiance.ALLY, e.pos);
    }

    public void OnUnitRenderedEvent(Unit unit)
    {
        this.unitMap.Add(unit.GameObject.name, unit);
    }

    // Used to control game logic like army growth etc.
    public List<TurnUpdate> TurnUpdate()
    {
        List<TurnUpdate> updates = new List<TurnUpdate>();

        // TODO: Add enemy soldiers here.
        if (false && Enemy.Soldiers.Count < 2)
        { 
            // TODO: Move this out of here. But for now, it's just a rough in.
            Soldier soldier = AddEnemySoldier();
            soldier.StartMoving();
            TurnUpdate tu = new TurnUpdate
            {
                Unit = soldier
            };
            updates.Add(tu);
        }
        return updates;
    }

    private Soldier AddEnemySoldier()
    {
        Vector2 pos = new Vector2
        {
            // TODO: This will blow up.
            x = Enemy.ArmyBase.Position.x,
            y = Enemy.ArmyBase.Position.y
        };
        Soldier soldier = new Soldier
        {
            Allegiance = Allegiance.ENEMY,
            Position = pos

        };
        soldier.Name = "EnemySoldier_" + Guid.NewGuid().ToString();
        soldier.Init();

        unitMap.Add(soldier.Name, soldier);

        //soldier.StartMoving();

        Enemy.Soldiers.Add(soldier);

        //TODO: Pass back to renderer.
        return soldier;

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

        // TODO: If statement here is bad. 
        List<Soldier> soldiers = (allegiance == Allegiance.ALLY ? Player.Soldiers : Enemy.Soldiers);
        soldier.Init();
        soldier.OnDestroyedEvent += Soldier_OnDestroyedEvent;
        soldiers.Add(soldier);
        return soldier;
    }

    public Soldier OnAddSoldier(GameObject gameObject, Allegiance allegiance)
    {
        Soldier soldier = new Soldier()
        {
            Allegiance = allegiance,
            GameObject = gameObject
        };

        // TODO: If statement here is bad. 
        List<Soldier> soldiers = (allegiance == Allegiance.ALLY ? Player.Soldiers : Enemy.Soldiers);
        soldier.Init();
        soldier.OnDestroyedEvent+= Soldier_OnDestroyedEvent;
        soldiers.Add(soldier);
        unitMap.Add(gameObject.name, soldier);
        return soldier;
    }

    void EnemyBase_OnDestroyedEvent(Unit destroyedUnit)
    {
        // TODO: This can be cleaned up DRY. 
        unitMap.Remove(destroyedUnit.GameObject.name);
        GameController.Log("You win.");
    }

    void Soldier_OnDestroyedEvent(Unit unitDestroyed)
    {
        // TODO: Feels like all the units and gameobjects can be managed at once? 
        Player.Soldiers.Remove(Player.Soldiers.Find((Soldier obj) => obj.GameObject.name == unitDestroyed.GameObject.name));
        unitMap.Remove(unitDestroyed.GameObject.name);
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
