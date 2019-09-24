using System;
using UnityEngine;
using static UnitGameEvents;

/// <summary>
/// Renders a soldier as a white box.
/// May just delete this. Part of early prototype.
/// </summary>
public class BoxSoldierRenderer : IUnitRenderer
{
    public static class ExplosionAnimation
    {
        public static readonly int ID = 1;
        public static readonly float TIME = 3f;
    }

    RectangleObject rectanglePrefab = null;
    GameObject allyPrefab = null;

    public BoxSoldierRenderer(RectangleObject rectanglePrefab)
    {
        this.rectanglePrefab = rectanglePrefab;
    }

    public BoxSoldierRenderer(GameObject allyPrefab)
    {
        this.allyPrefab = allyPrefab;
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

    MoveableObject IUnitRenderer.Draw(Vector2 position)
    {
        throw new NotImplementedException();
    }

    MoveableObject IUnitRenderer.Draw(Vector2 position, string name)
    {
        throw new NotImplementedException();
    }

    public void HandleEvent(UnitDyingEvent dyingEvent)
    {
        Animator animator = dyingEvent.Unit.GameObject.GetComponent<Animator>();
        animator.Play("Explosion");
    }

    public void HandleEvent(UnitDiedEvent unitDiedEvent)
    {
        // TODO: Object pooling at some point. 
        UnityEngine.Object.Destroy(unitDiedEvent.Unit.GameObject);
    }
}