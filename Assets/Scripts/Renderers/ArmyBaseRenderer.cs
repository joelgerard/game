using System;
using UnityEngine;

public class ArmyBaseRenderer
{
    // TODO: Why is this here?
    RectangleObject rectangle;

    public ArmyBaseRenderer(RectangleObject drawShape)
    {
        rectangle = drawShape;
    }

    public RectangleObject Draw(RectangleObject RectanglePrefab, Vector2 position, String name)
    {
        return rectangle.Draw(name, RectanglePrefab, position, 1.1f, 1.1f);
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