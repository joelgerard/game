using System;
using UnityEngine;
using static UnitGameEvents;

public class ArmyBaseRenderer
{
    readonly GameObject armyBasePrefab = null;

    public ArmyBaseRenderer(GameObject armyBasePrefab)
    {
        this.armyBasePrefab = armyBasePrefab;
    }

    public GameObject Draw(Vector2 position)
    {
        return Draw(position, "ArmyBase_" + Guid.NewGuid().ToString());
    }

    public GameObject Draw(Vector2 position, string name)
    {

        GameObject go = UnityEngine.Object.Instantiate(armyBasePrefab);
        go.transform.position = position;
        go.name = name;


        return go;
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
    }

    public void HandleEvent(UnitDyingEvent dyingEvent) => throw new NotImplementedException();

    public void HandleEvent(UnitDiedEvent unitDiedEvent) =>
        // TODO: Object pooling at some point. 
        throw new NotImplementedException();
}
