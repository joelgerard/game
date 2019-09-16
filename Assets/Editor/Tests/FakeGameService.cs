using System;
using System.Collections.Generic;
using UnityEngine;

public class FakeGameService
{
    Game game;

    public FakeGameService(Game game)
    {
        this.game = game;
    }

    public void RenderUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            RenderUnit(unit);
        }

    }

    public void RenderUnit(Unit unit)
    {
        GameObject go = new GameObject();
        go.name = (unit.Name ?? Guid.NewGuid().ToString());
        unit.GameObject = go;
        game.OnUnitRenderedEvent(unit);
    }
}
