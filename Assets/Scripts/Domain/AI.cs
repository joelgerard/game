using System;
using System.Collections.Generic;
using UnityEngine;
using static UnitGameEvents;

public class AI
{
    int soldierCounter = 0;

    public AI()
    {
    }

    public List<UnitEvent> GetAIInput(Game game, Army player, Army enemy)
    {
        List<UnitEvent> gameEvents = new List<UnitEvent>();

        soldierCounter++;
        if (soldierCounter == 50)
        {
            soldierCounter = 0;
        }
        if (soldierCounter == 0)
        {
            GameController.Log("AI soldier");
            // TODO: Icky.
            Unit soldier = game.AddSoldier(Allegiance.ENEMY, enemy.ArmyBase.Position);

            // TODO: Long winded.
            soldier.StateMachine.Transition(new MovingState(soldier));
            UnitCreatedEvent unitCreatedEvent = new UnitCreatedEvent
            {
                Unit = soldier
            };
            gameEvents.Add(unitCreatedEvent);
        }
        return gameEvents;
    }

    public List<Unit> Update(Game game, Army player, Army enemy)
    {
        List<Unit> units = new List<Unit>();
return units;

        System.Random r = new System.Random();
        double xOffset = r.NextDouble()/2;
        double yOffset = r.NextDouble()/2;

        Vector2 v = new Vector2(enemy.ArmyBase.Position.x + (float) xOffset,
            enemy.ArmyBase.Position.y + (float) yOffset
        );

        Soldier soldier = game.AddSoldier(Allegiance.ENEMY, v);
        soldier.StartMoving();
        units.Add(soldier);

        return units;
    }
}
