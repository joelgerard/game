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

        // TODO: Enemy doesn't move yet.
        Enemy.Update(update.deltaTime, null);
    }


    public Army AddArmy()
    {
        Army army = new Army();
        Armies.Add(army);
        return army;
    }

    void EnemyBase_OnDestroyedEvent(Unit destroyedUnit)
    {
        GameController.Log("You win.");
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
        soldiers.Add(soldier);
        unitMap.Add(gameObject.name, soldier);
        return soldier;
    }

    // TODO: Why does this take strings?
    public void OnUnitsCollide(String unit1Name, String unit2Name)
    {
        Unit unit = unitMap[unit1Name];
        Unit otherUnit = unitMap[unit2Name];
        unit.Attack(otherUnit);

        // TODO: This is wrong. Units shouldn't automatically counter-attack.
        otherUnit.Attack(unit);
    }
}
