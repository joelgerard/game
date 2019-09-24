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

        UnitsCollideEvent unitsCollideEvent = 
            new UnitsCollideEvent(this.gameObject.name, other.gameObject.name);
        OnCollisionEvent?.Invoke(unitsCollideEvent);
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
