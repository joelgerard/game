using System;
using UnityEngine;
// TODO: This whole class is a temp hack.

public class MoveableObjectWrapper
{
    private GameObject gameObject = null;
    public MoveableObject MoveableObject { get; set; }

    public GameObject GameObject
    {
        get
        {
            if (gameObject == null)
            {
                return MoveableObject.gameObject;
            }
            else
            {
                return gameObject;
            }
        }
        set
        {
            this.gameObject = value;
        }
    }

}
