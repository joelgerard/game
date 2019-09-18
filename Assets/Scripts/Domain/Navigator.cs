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
            //bool isMoving = soldier.StateMachine.IsInState<Moving>();
            bool isMoving = soldier.StateMachine.IsInState<MovingState>();

            Vector2 targetPos = soldier.TargetPosition;
            if (isMoving)
            {

                // TODO: This is awkward.
                if (soldier.IndexOnPath == 0)
                {
                    // FIXME: This pathRendere seems to start from 1? 
                    // What is it? 0 or 1. It means the limits of the array aren't being
                    // checked properly, e.g. in the case below. 
                    // There are also two paths, the pathRenderer, and the plain
                    // AI path so they need to match. 
                    if (soldier.IndexOnPath != -1 && soldier.IndexOnPath < Path.Count())
                    {
                        // TODO: Counts from 1?
                        targetPos = Path.GetPosition(0);
                    }
                }

                // How close is the soldier to its target?
                float distanceFromTarget = Vector2.Distance(soldier.Position, targetPos);

                // TODO: This distance from target, why is it here?
                if (distanceFromTarget > 0.01f)
                {
                    soldier.Position =
                        Vector2.MoveTowards(soldier.Position, targetPos, soldier.Speed * deltaTime);
                    GameController.Log("Moving soldier towards " + targetPos.ToString());
                }
                else
                {
                    int currentIndex = soldier.IndexOnPath; 
                    if (currentIndex != -1 && currentIndex < Path.Count())
                    {
                        soldier.TargetPosition = Path.GetPosition(soldier.IndexOnPath);
                        soldier.IndexOnPath++;
                        GameController.Log("Got pos to move to " + soldier.TargetPosition);
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

