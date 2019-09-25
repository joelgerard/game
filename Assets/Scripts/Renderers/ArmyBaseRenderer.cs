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
        try 
        { 
        Diagnostics.NotNull(armyBasePrefab, "armyBasePrefab in Draw");

        armyBasePrefab.SetActive(true);
        GameObject go = UnityEngine.Object.Instantiate(armyBasePrefab);

        Diagnostics.NotNull(go, "go in Draw");

        //go.GetComponent<Sprite>().
        go.SetActive(true);

        go.transform.position = position;
        go.name = name;


        return go;
        } catch( Exception e)
        {
            GameController.Log("Error rendering base "  + e.ToString());
            throw e;
        }
    }

    public void DrawDamage(Unit unitDamaged, float percentHealth)
    {
    }

    public void HandleEvent(UnitDyingEvent dyingEvent) => throw new NotImplementedException();

    public void HandleEvent(UnitDiedEvent unitDiedEvent) =>
        // TODO: Object pooling at some point. 
        throw new NotImplementedException();
}
