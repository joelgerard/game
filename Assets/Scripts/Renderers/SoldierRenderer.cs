using System;
using UnityEngine;

// TODO: What is this class for?
public class SoldierRenderer
{
    DrawShape CurrentShapeToDraw;

    public SoldierRenderer()
    {
    }

    public DrawShape Draw(DrawShape RectanglePrefab, Vector2 position)
    {
        DrawRectangle dr = new DrawRectangle();
        CurrentShapeToDraw = dr.Draw(RectanglePrefab, position, 0.1f, 0.1f);

        //var prefab = RectanglePrefab; //_drawModeToPrefab[Mode];
        //CurrentShapeToDraw = UnityEngine.Object.Instantiate(prefab);
        //CurrentShapeToDraw.name = "Soldier " + Guid.NewGuid().ToString(); // TODO: Update name // + _allShapes.Count;

        //CurrentShapeToDraw.AddVertex(position);
        //CurrentShapeToDraw.AddVertex(position);

        //position.x += 0.1f;
        //position.y += 0.1f;
        //CurrentShapeToDraw.AddVertex(position);


        //CurrentShapeToDraw.UpdateShape(position);

        return CurrentShapeToDraw;
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
        int i = 0;
    }
}
