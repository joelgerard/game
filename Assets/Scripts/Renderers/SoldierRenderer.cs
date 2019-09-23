using System;
using UnityEngine;
using static UnitGameEvents;

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

            Animator animator = go.GetComponent<Animator>();
            // TODO: Hard coded 1
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[1];
            AnimationEvent evt = new AnimationEvent();
            // TODO: Hard coded 1.
            evt.intParameter = 1;
            evt.time = 3f;
            evt.functionName = "AnimationEvent";

            // TODO: Why is this event hooked up here? Should be hooked up in object creation.
            clip.AddEvent(evt);
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