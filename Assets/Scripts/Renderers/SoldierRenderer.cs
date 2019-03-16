using System;
using UnityEngine;

// TODO: What is this class for?
public class SoldierRenderer
{
    RectangleObject rectangle;

    public SoldierRenderer()
    {
    }

    public RectangleObject Draw(RectangleObject RectanglePrefab, Vector2 position)
    {
        RectangleObject dr = new RectangleObject();
        rectangle = dr.Draw(RectanglePrefab, position, 0.1f, 0.1f,"Soldier");
        return rectangle;
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
