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

    // TODO: This should return commands to make the renderer do something.
    // e.g. switch unit to dying animation. 
    public FrameUpdate Update(GameUpdate update)
    {
        //List<Unit> createdUnits = new List<Unit>();
        FrameUpdate frameUpdate = new FrameUpdate();
        foreach(GameEvent curEvent in update.GameEvents)
        {
            GameController.Log("Handling event " + curEvent.GetType());

            // TODO: consider moving to different function.
            // FIXME: Dispatch this properly
            if (curEvent is HomeBaseClickEvent)
            {
                frameUpdate.AddCreatedEvent(HomeBaseClickedEvent(curEvent as HomeBaseClickEvent));
            }
            if (curEvent is UnitsCollideEvent)
            {
                // FIXME: WTF. OK. So all the event handling is happening in one place.
                // However, it takes forever to get there and the issue remains the same.
                // How do you move from one state to the next? 
                // <<>> 
                GameController.Log("Process units UnitsCollide");
                ((UnitsCollideEvent)curEvent).Unit.Attack(((UnitsCollideEvent)curEvent).OtherUnit);
            }
            if (curEvent is UnitExplosionComplete)
            {
                UnitExplosionComplete uec = (UnitExplosionComplete)curEvent;

                // TODO: Lazy. No dead state? 
                Unit unit = unitMap[uec.UnitName];
                UnityEngine.GameObject.Destroy(unit.GameObject);
                unitMap.Remove(uec.UnitName);
            }
        }
        // TODO: Need to call this once per frame?
        // NOTE: If attacking, this is called once per frame.
        List<UnitEvent> unitEvents = Player.Update(update.deltaTime, update.currentPath);
        frameUpdate.UnitEvents.AddRange(unitEvents);
        Enemy.Update(update.deltaTime, Path);
        

        return frameUpdate;
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
    public GameEvent OnUnitsCollide(String unit1Name, String unit2Name)
    {
        Diagnostics.KeyExists(unitMap, unit1Name);
        Diagnostics.KeyExists(unitMap, unit2Name);

        GameController.Log("Unit " + unit1Name + " collided with " + unit2Name);

        Unit unit = unitMap[unit1Name];
        Unit otherUnit = unitMap[unit2Name];

        return new UnitsCollideEvent(unit, otherUnit);
    }
}
