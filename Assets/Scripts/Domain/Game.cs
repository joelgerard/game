using System;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public List<Army> Armies { get; set; } = new List<Army>();
    public Army Player { get { return Armies[1]; } }
    public Army Enemy { get { return Armies[0]; } }

    // TODO: Make private
    public Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();

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

    public void Update(GameUpdate update, float deltaTime)
    {
        // TODO: Need to call this once per frame?
        Player.Update(deltaTime, update.currentPath);
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

    public void OnAddSoldier(GameObject gameObject)
    {
        Soldier soldier = new Soldier(gameObject)
        {
            Allegiance = Allegiance.ALLY
        };
        
        List<Soldier> soldiers = Player.Soldiers;
        soldier.Init();
        soldiers.Add(soldier);
        unitMap.Add(gameObject.name, soldier);
    }

    public void OnUnitsCollide(String unit1Name, String unit2Name)
    {
        Unit unit = unitMap[unit1Name];
        Unit otherUnit = unitMap[unit2Name];
        unit.Attack(otherUnit);
    }
}
