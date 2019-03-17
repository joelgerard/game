using System;
using UnityEngine;

public class ArmyBaseRenderer : IUnitRenderer
{
    RectangleObject rectanglePrefab;

    public ArmyBaseRenderer(RectangleObject rectanglePrefab)
    {
        this.rectanglePrefab = rectanglePrefab;
    }

    public MovingObject Draw(Vector2 position, String name)
    {
        return rectanglePrefab.Draw(name, rectanglePrefab, position, 1.1f, 1.1f);
    }

    public MovingObject Draw(Vector2 position)
    {
        return Draw(position, "Base_" + Guid.NewGuid().ToString());
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
        RectangleObject.SetColorRed(unitDamaged.GameObject,1- percentHealth);
    }

    public void DrawDestroyed(Unit unit)
    {
        UnityEngine.Object.Destroy(unit.GameObject);
    }
}