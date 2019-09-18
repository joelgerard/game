using System;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    public AI()
    {
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
