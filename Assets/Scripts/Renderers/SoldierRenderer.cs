using System;
using UnityEngine;

public class SoldierRenderer : IUnitRenderer
{
    RectangleObject rectanglePrefab;

    public SoldierRenderer(RectangleObject rectanglePrefab)
    {
        this.rectanglePrefab = rectanglePrefab;
    }

    public MoveableObject Draw(Vector2 position)
    {
        return Draw(position, "Soldier_" + Guid.NewGuid().ToString());
    }

    public MoveableObject Draw(Vector2 position, string name)
    {
        RectangleObject dr = new RectangleObject();
        return dr.Draw(name, this.rectanglePrefab, position, 0.1f, 0.1f);
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
        RectangleObject.SetColorRed(unitDamaged.GameObject, 1 - percentHealth);
    }

    public void DrawDestroyed(Unit unit)
    {
        UnityEngine.Object.Destroy(unit.GameObject);
    }
}
