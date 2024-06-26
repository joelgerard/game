﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public abstract class MoveableObject : MonoBehaviour
{
    public event OnEnter OnEnterEvent;

    public delegate void OnEnter(GameObject thisObject, GameObject otherObject);

    private void OnTriggerEnter2D(Collider2D other)
    {
        Diagnostics.NotNull(other, "other");
        Diagnostics.NotNull(other.gameObject, "other.gameObject");
        Diagnostics.NotNull(this.gameObject, "this.gameObject");

        OnEnterEvent?.Invoke(this.gameObject, other.gameObject);
    }
}