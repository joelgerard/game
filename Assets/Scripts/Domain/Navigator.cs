using System;
using System.Collections.Generic;
using UnityEngine;
using static MovableUnit.MovableUnitHsm;
using static Soldier.SoldierHsm;

public class Navigator
{
    public Navigator()
    {

    }

    // TODO: Should this be in a state update for the army?
    // the state update has delta time. 
    public void MoveUnits(float deltaTime, List<Soldier> soldiers, IPath Path)
    {
        if (Path == null)
        {
            return;
        }
        int numPos = Path.Count();

        foreach (Soldier soldier in soldiers)
        {
            bool isMoving = soldier.StateMachine.IsInState<Moving>();

            Vector2 targetPos = soldier.TargetPosition;
            if (isMoving)
            {

                // TODO: This is awkward.
                if (soldier.IndexOnPath == 0)
                {
                    if (soldier.IndexOnPath != -1 && soldier.IndexOnPath < Path.Count() - 1)
                    {
                        targetPos = Path.GetPosition(1);
                    }
                }

                // How close is the soldier to its target?
                float distanceFromTarget = Vector2.Distance(soldier.Position, targetPos);

                // TODO: This distance from target, why is it here?
                if (distanceFromTarget > 0.01f)
                {
                    soldier.Position =
                        Vector2.MoveTowards(soldier.Position, targetPos, soldier.Speed * deltaTime);
                }
                else
                {
                    int currentIndex = soldier.IndexOnPath; 
                    if (currentIndex != -1 && currentIndex < Path.Count()-1)
                    {
                        soldier.TargetPosition = Path.GetPosition(++soldier.IndexOnPath);
                    }
                    else 
                    {
                        // TODO: Start here <<>>. Soldier never stops moving at the end
                        // of this path. 
                        GameController.Log("Stop moving?"); 
                    }
                }

            }
        }
    }
}

