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
    public void MoveUnits(float deltaTime, List<Soldier> soldiers, IPath Path)
    {
        if (Path == null)
        {
            return;
        }
        int numPos = Path.Count();

        // TODO: This is wrong. Should be, if the unit has arrived, then 
        // keep moving as long as there are more points to move towards.
        //if (numPos > 0 && currentPos < numPos)
        //{
        //Vector2 targetPos = Path.GetPosition(currentPos);

        foreach (Soldier soldier in soldiers)
        {
            bool isMoving = soldier.StateMachine.IsInState<Moving>();
            Vector2 targetPos = soldier.TargetPosition;
            if (isMoving)
            {
                GameController.Log("moving soldier to " + targetPos.ToString());

                // How close is the soldier to its target?
                float distanceFromTarget = Vector2.Distance(soldier.Position, targetPos);

                if (distanceFromTarget > 0.01f)
                {
                    soldier.Position =
                        Vector2.MoveTowards(soldier.Position, targetPos, soldier.Speed * deltaTime);
                }
                else
                {
                    // Next target?
                    int currentIndex = FindIndex(Path, soldier.Position);
                    if (currentIndex != -1 && currentIndex < Path.Count()-1)
                    {
                        soldier.TargetPosition = Path.GetPosition(currentIndex + 1);
                    }
                }

            }
            // are any of the shapes near the current pos? If so move to the next one
            /*if (Vector2.Distance(targetPos, soldier.Position) < 0.5)
            {
                currentPos++;
                // TODO: what if two units make it? fix. move out of for loop
            }*/
        }
        //}
    }

    // TODO: omg, so horrible. Fix.
    int FindIndex(IPath path, Vector2 pos)
    {
        int index = 0;
        while(index < path.Count())
        {
            if (pos == path.GetPosition(index))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    // TODO: Should this be in a state update for the army?
    // the state update has delta time. 
    /*public void MoveUnits(float deltaTime, List<Soldier> soldiers, IPath Path)
    {
        if (Path == null)
        {
            return;
        }
        int numPos = Path.Count();

        // TODO: This is wrong. Should be, if the unit has arrived, then 
        // keep moving as long as there are more points to move towards.
        if (numPos > 0 && currentPos < numPos)
        {
            Vector2 targetPos = Path.GetPosition(currentPos);

            foreach (Soldier soldier in soldiers)
            {
                bool isMoving = soldier.StateMachine.IsInState<Moving>(); 
                if (isMoving) 
                {


                    soldier.Position =
                            Vector2.MoveTowards(soldier.Position, targetPos, soldier.Speed * deltaTime);
                }
                // are any of the shapes near the current pos? If so move to the next one
                if (Vector2.Distance(targetPos, soldier.Position) < 0.5)
                {
                    currentPos++;
                    // TODO: what if two units make it? fix. move out of for loop
                }
            }
        }
    }*/
}

