using System;
using System.Collections.Generic;
using UnityEngine;
using static MovableUnit.MovableUnitHsm;
using static Soldier.SoldierHsm;

public class Navigator
{
    private int currentPos = 0;

    public Navigator()
    {

    }

    // TODO: Should this be in a state update for the army?
    // the state update has delta time. 
    public void MoveUnits(List<Soldier> soldiers, IPath Path)
    {
        if (Path == null)
        {
            return;
        }
        //Debug.Log("Moving?");
        int numPos = Path.Count();
        if (numPos > 0 && currentPos < numPos)
        {
            //Debug.Log("Move all soldiers") ;
            Vector2 targetPos = Path.GetPosition(currentPos);

            foreach (Soldier soldier in soldiers)
            {
                bool isMoving = soldier.StateMachine.IsInState<Moving>(); 
                Debug.Log("Moving soldier " + isMoving);
                if (isMoving) 
                {

                    // todo: what happens in test to delta time?
                    soldier.Position =
                            Vector2.MoveTowards(soldier.Position, targetPos, 3 * Time.deltaTime);
                }
                // are any of the shapes near the current pos? If so move to the next one
                if (Vector2.Distance(targetPos, soldier.Position) < 0.5)
                {
                    currentPos++;
                    // TODO: what if two units make it? fix. move out of for loop
                }
            }
        }
    }
}

