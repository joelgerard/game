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

    public FrameUpdate Update(GameUpdate update)
    {
        FrameUpdate frameUpdate = new FrameUpdate();
        foreach(dynamic curEvent in update.GameEvents)
        {
            HandleEvent(curEvent, frameUpdate);
        }

        // TODO: Need to call this once per frame?
        // NOTE: If attacking, this is called once per frame.
        List<UnitEvent> unitEvents = Player.Update(update.deltaTime, update.currentPath);
        frameUpdate.UnitEvents.AddRange(unitEvents);
        Enemy.Update(update.deltaTime, Path);
        
        return frameUpdate;
    }

    private void HandleEvent(HomeBaseClickEvent e, FrameUpdate frameUpdate)
    {
        Unit soldier = AddSoldier(Allegiance.ALLY, e.pos);
        frameUpdate.AddCreatedEvent(soldier);
    }

    private void HandleEvent(UnitsCollideEvent e, FrameUpdate frameUpdate)
    {
        unitMap[e.Unit].Attack(unitMap[e.OtherUnit]);
    }

    private void HandleEvent(UnitExplosionComplete e, FrameUpdate frameUpdate)
    {
        // TODO: Lazy. No dead state? Also, the UnityEngine piece should be removed.
        Unit unit = unitMap[e.UnitName];
        UnityEngine.Object.Destroy(unit.GameObject);
        unitMap.Remove(e.UnitName);
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

    public void OnUnitRenderedEvent(Unit unit)
    {
        // TODO: Think about this naming.
        unit.Name = unit.GameObject.name;

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
        GameController.Log("Path ready");
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
        soldiers.Add(soldier);
        return soldier;
    }

    void EnemyBase_OnDestroyedEvent(Unit destroyedUnit)
    {
        // TODO: This can be cleaned up DRY. 
        //unitMap.Remove(destroyedUnit.GameObject.name);
        unitMap.Remove(destroyedUnit.Name);

        GameController.Log("You win.");
    }

}
