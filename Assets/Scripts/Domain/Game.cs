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
    private Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();

    public Game()
    {
    }

    public void Initialize()
    {

        // Enemy
        AddArmy();
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
    }



    public void Update(GameUpdate update)
    {
        // TODO: Need to call this once per frame?
        Player.Update(update.deltaTime, update.currentPath);
        Enemy.Update(update.deltaTime, Path);
    }

    // Used to control game logic like army growth etc.
    public List<TurnUpdate> TurnUpdate()
    {
        List<TurnUpdate> updates = new List<TurnUpdate>();

        if (Enemy.Soldiers.Count < 2)
        { 
            // TODO: Move this out of here. But for now, it's just a rough in.
            Soldier soldier = AddEnemySoldier();
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

    public Soldier OnAddSoldier(GameObject gameObject)
    {
        Soldier soldier = new Soldier()
        {
            Allegiance = Allegiance.ALLY,
            GameObject = gameObject
        };
        
        List<Soldier> soldiers = Player.Soldiers;
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
