using System;
using UnityEngine;

public class SoldierRenderer : IUnitRenderer
{
    RectangleObject rectanglePrefab = null;
    GameObject allyPrefab = null;

    public SoldierRenderer(RectangleObject rectanglePrefab)
    {
        this.rectanglePrefab = rectanglePrefab;
    }

    public SoldierRenderer(GameObject allyPrefab)
    {
        this.allyPrefab = allyPrefab; 
    }

    public MoveableObjectWrapper Draw(Vector2 position)
    {
        return Draw(position, "Soldier_" + Guid.NewGuid().ToString());
    }

    public MoveableObjectWrapper Draw(Vector2 position, string name)
    {
        // TODO: This is new and needs to be worked out.
        MoveableObjectWrapper obw = new MoveableObjectWrapper();
        if (allyPrefab != null)
        {
            GameObject go = UnityEngine.Object.Instantiate(allyPrefab);
            go.transform.position = position;
            go.name = name;
            obw.GameObject = go;
        }
        else
        {
            RectangleObject dr = new RectangleObject();
            obw.MoveableObject = dr.Draw(name, this.rectanglePrefab, position, 0.1f, 0.1f);
        }
        return obw;
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
        RectangleObject.SetColorRed(unitDamaged.GameObject, 1 - percentHealth);
    }

    public void DrawDestroyed(Unit unit)
    {
        UnityEngine.Object.Destroy(unit.GameObject);
    }

    MoveableObject IUnitRenderer.Draw(Vector2 position)
    {
        throw new NotImplementedException();
    }

    MoveableObject IUnitRenderer.Draw(Vector2 position, string name)
    {
        throw new NotImplementedException();
    }
}
