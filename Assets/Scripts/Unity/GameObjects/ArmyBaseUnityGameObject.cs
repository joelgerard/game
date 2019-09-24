using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyBaseUnityGameObject : MonoBehaviour
{
    public event OnCollision OnCollisionEvent;
    public delegate void OnCollision(UnityGameEvent unityGameEvent);

    // Start is called before the first frame update
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
}
