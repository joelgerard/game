using System;
using UnityEngine;

public class BoxArmyBaseRenderer : IUnitRenderer
{
    RectangleObject rectanglePrefab;

    public BoxArmyBaseRenderer(RectangleObject rectanglePrefab)
    {
        this.rectanglePrefab = rectanglePrefab;
    }

    public MoveableObject Draw(Vector2 position, string name)
    {
        return rectanglePrefab.Draw(name, rectanglePrefab, position, 1.1f, 1.1f);
    }

    public MoveableObject Draw(Vector2 position)
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