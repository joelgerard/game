﻿using UnityEngine;
using System.Collections;
using System;
using static SoldierRenderer;

// TODO: This is a general unit? Clean up.
// TODO: Not really a controller? Think about this. 
public class SoldierGameObject : MonoBehaviour
{
    int animationEvents=0;

    // TODO: Bad naming. This is really a collision event. 
    public event OnCollision OnCollisionEvent;
    public delegate void OnCollision(GameObject thisObject, GameObject otherObject);

    public event OnAnimation OnAnimationEvent;
    public delegate void OnAnimation(GameObject gameObject, int animationId);

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

        OnCollisionEvent?.Invoke(this.gameObject, other.gameObject);
    }

    private void AnimationEvent(int animationId)
    {
        // FIXME: WHY called twice????!!
        if (animationId == ExplosionAnimation.ID && animationEvents < 1) 
        {
            OnAnimationEvent?.Invoke(this.gameObject, animationId);
        }
        animationEvents++;
    }
}
