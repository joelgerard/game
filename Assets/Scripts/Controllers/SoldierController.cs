using UnityEngine;
using System.Collections;
using System;

// TODO: This is a general unit? Clean up.
public class SoldierController : MonoBehaviour
{
    // TODO: Bad naming. This is really a collision event. 
    public event OnEnter OnEnterEvent;
    public delegate void OnEnter(GameObject thisObject, GameObject otherObject);

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

        OnEnterEvent?.Invoke(this.gameObject, other.gameObject);
    }

    private void AnimationEvent(int animationId)
    {
        // <<>>
        throw new Exception("Unimplemented");    
    }
}
