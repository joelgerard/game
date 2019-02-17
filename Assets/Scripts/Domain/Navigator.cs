using System;
using System.Collections.Generic;
using UnityEngine;

public class Navigator
{
    private int currentPos = 0;

    public Navigator()
    {

    }

    public IPath Path { get; set; }

    public Vector2 GetPoint(int index)
    {
        return new Vector2();
    }

    public Vector2 GetTargetPosition(Vector2 currentPosition)
    {
        return new Vector2();
    }

    public void MoveUnits(List<Soldier> soldiers)
    {
        if (Path == null)
        {
            Debug.Log("Move units has path " + (Path != null));
            return;
        }
        int numPos = Path.Count();
        if (numPos > 0 && currentPos < numPos)
        {
            Debug.Log("Moving");
            Vector2 targetPos = Path.GetPosition(currentPos);

            foreach (Soldier soldier in soldiers)
            {
                soldier.Position =
                        Vector2.MoveTowards(soldier.Position, targetPos, 3 * Time.deltaTime);

                // are any of the shapes near the current pos? If so move to the next one
                if (Vector2.Distance(targetPos, soldier.Position) < 0.5)
                {
                    currentPos++;
                    // TODO: what if two units make it? fix
                }
            }
        }

    }
}

