using System;
using UnityEngine;

public class ArmyBaseRenderer
{
    DrawShape CurrentShapeToDraw;

    public ArmyBaseRenderer(DrawShape drawShape)
    {
        CurrentShapeToDraw = drawShape;
    }

    public DrawShape Draw(DrawShape RectanglePrefab, Vector2 position, String name)
    {
        // TODO: This is wrong. Don't call new here.
        //DrawRectangle dr = new DrawRectangle();
        //CurrentShapeToDraw = dr.Draw(RectanglePrefab, position, 1.1f, 1.1f);

        // TODO: unsafe cast
        ((DrawRectangle)CurrentShapeToDraw).Draw(name, RectanglePrefab, position, 1.1f, 1.1f);
        return CurrentShapeToDraw;
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
        // TODO: This is wrong. Don't call new here.
        //DrawRectangle dr = new DrawRectangle();

        // TODO: unsafe cast
        DrawRectangle dr = ((DrawRectangle)CurrentShapeToDraw);
        dr.SetColorRed(unitDamaged.GameObject,1- percentHealth);
    }
}