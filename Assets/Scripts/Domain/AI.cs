using System;
using System.Collections.Generic;

public class AI
{
    public AI()
    {
    }

    public List<Unit> Update(Game game, Army player, Army enemy)
    {
        List<Unit> units = new List<Unit>();

        Soldier soldier = game.AddSoldier(Allegiance.ENEMY, enemy.ArmyBase.Position);
        units.Add(soldier);

        return units;
    }
}
