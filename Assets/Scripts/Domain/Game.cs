using System;
using System.Collections.Generic;
using UnityEngine;
using static UnitGameEvents;

// This is the game as if it were a board game, i.e. no unity runtime stuff here.
// Just logic. Should be able to instantiate and test this anywhere. 
public class Game
{
    public List<Army> Armies { get; set; } = new List<Army>();
    public Army Player { get { return Armies[1]; } }
    public Army Enemy { get { return Armies[0]; } }
    public AI AI { get; set; } = new AI();

    /// <summary>
    /// Because, yeah, il2cpp doesn't support Dynamic
    /// </summary>
    Dictionary<Type, Func<UnityGameEvent, GameUpdateResult, UnitEvent>> UnityGameEventHandlers;


    // TODO: This path is hard coded for now.
    public Path Path { get; set; } = new Path();

    // TODO: I guess string comp is expensive. 
    // TODO: Why is this here? 
    public Dictionary<string, Unit> unitMap = new Dictionary<string, Unit>();

    public Game()
    {
        UnityGameEventHandlers = new Dictionary<Type, Func<UnityGameEvent, GameUpdateResult, UnitEvent>>
        {

            {typeof (ClickInPlayerBaseEvent), (gameEvent,gameUpdate) => 
                this.HandleEvent((ClickInPlayerBaseEvent)gameEvent,gameUpdate) },
            {typeof (UnitsCollideEvent), (gameEvent,gameUpdate) => 
                this.HandleEvent((UnitsCollideEvent)gameEvent,gameUpdate) },
            {typeof (UnitExplosionComplete), (gameEvent,gameUpdate) => 
                this.HandleEvent((UnitExplosionComplete)gameEvent,gameUpdate) }
        };
    }

    public void Initialize()
    {

        //Enemy
        AddArmy();

        // Player
        AddArmy(); //TODO: Not yet used.
    }

    public GameUpdateResult Update(GameUpdate update)
    {
        GameUpdateResult frameUpdate = new GameUpdateResult();

        // il2cpp doesn't support dynamic keyword :( :( :(
        //foreach (dynamic curEvent in update.UnityGameEvents)
        foreach(UnityGameEvent curEvent in update.UnityGameEvents)
        {
            if (curEvent != null)
            {
                UnitEvent unitEvent = UnityGameEventHandlers[curEvent.GetType()](curEvent, frameUpdate);
                if (unitEvent != null)
                {
                    frameUpdate.UnitEvents.Add(unitEvent);
                }
            }


        }
        update.UnityGameEvents.Clear();
        // TODO: Need to call this once per frame?
        // NOTE: If attacking, this is called once per frame.
        try
        {
            frameUpdate.UnitEvents.AddRange(Player.Update(update.deltaTime, update.currentPath));

        }
        catch (Exception err)
        {
            GameController.Log("Player update failed " + err.ToString());
            throw err;
        }
        try
        {
            frameUpdate.UnitEvents.AddRange(Enemy.Update(update.deltaTime, Path));

        }
        catch (Exception err)
        {
            GameController.Log("Enemy update failed " + err.ToString());
            throw err;
        }

        return frameUpdate;
    }

    // Used to control game logic like army growth etc.
    // TODO: Not sure this should return same class as UpdatE()
    public GameUpdateResult TurnUpdate()
    {
        GameUpdateResult turnUpdate = new GameUpdateResult();
        turnUpdate.UnitEvents.AddRange(AI.GetAIInput(this, Player, Enemy));
        return turnUpdate;
    }

    private UnitEvent HandleEvent(ClickInPlayerBaseEvent e, GameUpdateResult frameUpdate)
    {
        Unit soldier = AddSoldier(e.allegiance, e.pos);
        UnitCreatedEvent unitCreatedEvent = new UnitCreatedEvent
        {
            Unit = soldier
        };
        return unitCreatedEvent;
    }

    // TODO: Shouldn't these go to the unit directly?
    // They should because it's hard to know if a base or soldier is killed
    private UnitEvent HandleEvent(UnitsCollideEvent e, GameUpdateResult frameUpdate)
    {
        unitMap[e.Unit].Attack(unitMap[e.OtherUnit]);
        return null;
    }

    private UnitEvent HandleEvent(UnitExplosionComplete e, GameUpdateResult frameUpdate)
    {
        Unit unit = unitMap[e.UnitName];
        unitMap.Remove(e.UnitName);
        return unit.StateMachine.Transition(new DeadState(unit)).GetAssociatedEvent();
    }



    // TODO: Public?
    public List<Unit> DrawMap()
    {
        // Enemy
        Enemy.ArmyBase = CreateBase(Allegiance.ENEMY, new Vector2(0f, 3f));
        Enemy.ArmyBase.Name = "EnemyBaseSquare";

        // FIXME: This event is removed.
        //Enemy.ArmyBase.OnDestroyedEvent += EnemyBase_OnDestroyedEvent;

        // Enemy
        //Player.ArmyBase = CreateBase(Allegiance.ALLY, new Vector2(0f, -3f));
        Player.ArmyBase = CreateBase(Allegiance.ALLY, new Vector2(0f, 0f));
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

        // FIXME: This is broken.
        GameController.Log("You win.");
    }

}
