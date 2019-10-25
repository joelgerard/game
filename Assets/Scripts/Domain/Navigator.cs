using System;
using System.Collections.Generic;
using UnityEngine;
using static Map;
using static MovableUnit.MovableUnitHsm;

public class Navigator
{
    public Navigator()
    {

    }

    // TODO: Should this be in a state update for the army?
    // the state update has delta time. 
    public void MoveUnits(float deltaTime, List<Soldier> soldiers, IPath Path, Map map)
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
            bool isTracking = soldier.StateMachine.IsInState<TrackingState>();

            Vector2 targetPos = soldier.TargetPosition;
            if (isMoving || isTracking)
            {

                // TODO: This is awkward.
                if (soldier.IndexOnPath == 0 && isMoving)
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

                if (isTracking)
                {
                    if (soldier.Enemy.GameObject == null)
                    {
                        GameController.LogWarning("Soldier (" + soldier.Name + ") has enemy with name " + soldier.Enemy.Name + " is bad. It's state is " + soldier.Enemy.StateMachine.CurrentState);
                    }
                    targetPos = soldier.Enemy.GameObject.transform.position;
                }

                // How close is the soldier to its target?
                float distanceFromTarget = Vector2.Distance(soldier.Position, targetPos);

                // TODO: This distance from target, why is it here?
                if (distanceFromTarget > 0.01f)
                {

                    MapTileType mapTile = map.Get(soldier.Position);

                    // TODO: Temp. Remove.
                    if (soldier.GameObject != null)
                    {
                        SpriteRenderer m_SpriteRenderer = soldier.GameObject.GetComponent<SpriteRenderer>();
                        switch (mapTile)
                        {
                            case Map.MapTileType.Forest:
                                m_SpriteRenderer.color = Color.black;
                                break;
                            case Map.MapTileType.Grass:
                                m_SpriteRenderer.color = Color.green;
                                break;
                            case Map.MapTileType.Road:
                                m_SpriteRenderer.color = Color.yellow;
                                break;
                        }
                    }
                    float soldierSpeed = soldier.GetSpeed(mapTile);
                    soldier.Position =
                        Vector2.MoveTowards(soldier.Position, targetPos, soldierSpeed * deltaTime);
                    //GameController.Log("Moving soldier towards " + targetPos.ToString());
                }
                else if (isMoving)
                {
                    int currentIndex = soldier.IndexOnPath; 
                    if (currentIndex != -1 && currentIndex < Path.Count())
                    {
                        soldier.TargetPosition = Path.GetPosition(soldier.IndexOnPath);
                        soldier.IndexOnPath++;
                        //GameController.Log("Got pos to move to " + soldier.TargetPosition);
                    }
                    else 
                    {
                        // TODO: Start here <<>>. Soldier never stops moving at the end
                        // of this path.    
                        //GameController.Log("Stop moving?"); 
                    }
                }

            }

        }
    }
}

