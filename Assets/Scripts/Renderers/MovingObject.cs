using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public abstract class MovingObject : MonoBehaviour
{
    public event OnEnter OnEnterEvent;

    public delegate void OnEnter(GameObject thisObject, GameObject otherObject);

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnEnterEvent(this.gameObject, other.gameObject);
        if (true)
        {

            Debug.Log("entered" + other.name + " _ " + other.tag);
        }
    }

}