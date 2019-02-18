using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class MovingObject : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (true)
        {
            Debug.Log("entered" + other.name + " _ " + other.tag);
        }
    }

}