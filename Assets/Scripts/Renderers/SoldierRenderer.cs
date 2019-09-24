using System;
using UnityEngine;
using static UnitGameEvents;

public class SoldierRenderer
{

    public static class ExplosionAnimation
    {
        public static readonly int ID = 1;
        public static readonly float TIME = 3f;
    }

    readonly GameObject soldierPrefab = null;

    public SoldierRenderer(GameObject soldierPrefab)
    {
        this.soldierPrefab = soldierPrefab;
    }

    public GameObject Draw(Vector2 position)
    {
        return Draw(position, "Soldier_" + Guid.NewGuid().ToString());
    }

    public GameObject Draw(Vector2 position, string name)
    {

        GameObject go = UnityEngine.Object.Instantiate(soldierPrefab);
        go.transform.position = position;
        go.name = name;

        Animator animator = go.GetComponent<Animator>();
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[ExplosionAnimation.ID];
        AnimationEvent evt = new AnimationEvent();
        evt.intParameter = ExplosionAnimation.ID;
        evt.time = ExplosionAnimation.TIME;
        evt.functionName = "AnimationEvent";

        clip.AddEvent(evt);
        return go;
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
        //RectangleObject.SetColorRed(unitDamaged.GameObject, 1 - percentHealth);
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
