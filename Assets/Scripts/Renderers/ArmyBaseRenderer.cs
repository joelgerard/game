using System;
using UnityEngine;

public class ArmyBaseRenderer
{
    DrawShape CurrentShapeToDraw;

    public ArmyBaseRenderer()
    {
    }

    public DrawShape Draw(DrawShape RectanglePrefab, Vector2 position)
    {
        DrawRectangle dr = new DrawRectangle();
        CurrentShapeToDraw = dr.Draw(RectanglePrefab, position, 1.1f, 1.1f);
        return CurrentShapeToDraw;
    }

    public void DrawDamage(float health)
    {

    }

}
