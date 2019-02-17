using System;
using UnityEngine;

public class SoldierRenderer
{
    public SoldierRenderer()
    {
    }

    public MonoBehaviour Draw(DrawShape RectanglePrefab, Vector2 position)
    {
        DrawShape CurrentShapeToDraw;
        var prefab = RectanglePrefab; //_drawModeToPrefab[Mode];
        CurrentShapeToDraw = UnityEngine.Object.Instantiate(prefab);
        CurrentShapeToDraw.name = "Shape "; // TODO: Update name // + _allShapes.Count;

        CurrentShapeToDraw.AddVertex(position);
        CurrentShapeToDraw.AddVertex(position);

        position.x += 0.1f;
        position.y += 0.1f;
        CurrentShapeToDraw.AddVertex(position);

        //IsDrawingShape = true;
        CurrentShapeToDraw.SimulatingPhysics = false;


        CurrentShapeToDraw.UpdateShape(position);
        //CurrentShapeToDraw = null;
        //IsDrawingShape = false;
        return CurrentShapeToDraw;
    }
}
