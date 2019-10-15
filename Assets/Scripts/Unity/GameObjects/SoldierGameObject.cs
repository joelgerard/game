using UnityEngine;
using System.Collections;
using System;
using static SoldierRenderer;

// TODO: This is a general unit? Clean up.
// TODO: Not really a controller? Think about this. 
public class SoldierGameObject : MonoBehaviour
{
    int animationEvents=0;

    public event OnCollision OnCollisionEvent;
    public delegate void OnCollision(UnityGameEvent unityGameEvent);

    public event OnAnimation OnAnimationEvent;
    public delegate void OnAnimation(UnityGameEvent unityGameEvent);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Diagnostics.NotNull(other, "other");
        Diagnostics.NotNull(other.gameObject, "other.gameObject");
        Diagnostics.NotNull(this.gameObject, "this.gameObject");

        CircleCollider2D sightCollider = this.GetComponent<CircleCollider2D>();
        BoxCollider2D fightCollider = this.GetComponent<BoxCollider2D>();

        CircleCollider2D otherSightCollider = other.GetComponent<CircleCollider2D>();
        BoxCollider2D otherFightCollider = other.GetComponent<BoxCollider2D>();


        // TODO: This sightcollider is duplicated in the base.
        if (sightCollider!= null && otherFightCollider != null && sightCollider.IsTouching(otherFightCollider))
        {
            UnitsCollideEvent unitsCollideEvent =
                new UnitsCollideEvent(this.gameObject.name, other.gameObject.name, UnitsCollideEvent.CollisionEventType.SIGHT);
            OnCollisionEvent?.Invoke(unitsCollideEvent);
        }

        if (fightCollider != null && otherFightCollider != null && fightCollider.IsTouching(otherFightCollider))
        {
            UnitsCollideEvent unitsCollideEvent = 
                new UnitsCollideEvent(this.gameObject.name, other.gameObject.name,UnitsCollideEvent.CollisionEventType.ATTACK);
            OnCollisionEvent?.Invoke(unitsCollideEvent);
        }

        // TODO: What happens if none of the above trigger? I would say nothing, e.g. SIGHT intersects SIGHT. 
        // TODO: Is is okay if this happens multiple times per frame?
    }

    private void AnimationEvent(int animationId)
    {
        // FIXME: WHY called twice????!!
        if (animationId == ExplosionAnimation.ID && animationEvents < 1) 
        {
            UnitExplosionComplete gameEvent = new UnitExplosionComplete
            {
                UnitName = this.name
            };
            OnAnimationEvent?.Invoke(gameEvent);

        }
        animationEvents++;
    }
}
