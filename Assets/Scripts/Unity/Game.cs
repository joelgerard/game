using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    Navigator navigator = new Navigator();
    public List<Army> Armies {get;set;} = new List<Army>();
    public List<Soldier> soldiers = new List<Soldier>();
    Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();
    public DrawShape RectanglePrefab;
    public DrawShape CirclePrefab;
    public DrawShape TrianglePrefab;

    public void Initialize(DrawShape rectanglePrefab, DrawShape circlePrefab, DrawShape trianglePrefab)
    {
        this.RectanglePrefab = rectanglePrefab;
        this.TrianglePrefab = trianglePrefab;
        this.CirclePrefab = circlePrefab;

        // Enemy
        AddArmy();
        DrawRectangle enemyBaseSquare = GameObject.Find("EnemyBaseSquare").gameObject.GetComponent<DrawRectangle>();
        ArmyBase enemyBase = new ArmyBase
        {
            Allegiance = Allegiance.ENEMY
        };
        enemyBase.Init();
        unitMap.Add(enemyBaseSquare.name, enemyBase);


        // Player
        AddArmy(); //TODO: Not yet used.

        DrawRectangle launchPad = GameObject.Find("PlayerBaseSquare").gameObject.GetComponent<DrawRectangle>();
        ArmyBase playerBase = new ArmyBase
        {
            Allegiance = Allegiance.ALLY
        };
        playerBase.Init();
        unitMap.Add(launchPad.name, playerBase);


    }

    public void Update(GameUpdate update)
    {
        foreach (Soldier soldier in soldiers)
        {
            soldier.StateMachine.ProcessStateTransitions();
        }
        navigator.MoveUnits(soldiers, update.currentPath);
    }

    public void AddSoldier(Vector2 position)
    {
        SoldierRenderer sr = new SoldierRenderer();
        DrawShape soldierMono = sr.Draw(RectanglePrefab, position);

        // TODO: Is this needed?
        //_allShapes.Add(soldierMono);
        soldierMono.OnEnterEvent += UnitMono_OnEnterEvent;

        Soldier soldier = new Soldier(soldierMono.gameObject)
        {
            Allegiance = Allegiance.ALLY
        };
        soldier.Init();
        soldiers.Add(soldier);
        unitMap.Add(soldierMono.name, soldier);
        Debug.Log("AddSolder " + soldiers.Count);
    }

    void UnitMono_OnEnterEvent(GameObject thisObject, GameObject otherObject)
    {

        Unit unit = unitMap[thisObject.name];
        Unit otherUnit = unitMap[otherObject.name];
        unit.Attack(otherUnit);
    }

    public Army Player { get { return Armies[1]; } }
    public Army Enemy { get { return Armies[0]; } }

    public Army AddArmy()
    {
        Army army = new Army();
        Armies.Add(army);
        return army;
    }
}
