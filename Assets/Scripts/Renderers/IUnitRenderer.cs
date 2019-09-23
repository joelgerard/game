using System;
using UnityEngine;

// TODO: What's this for?
public interface IUnitRenderer
{
    MoveableObject Draw(Vector2 position);
    MoveableObject Draw(Vector2 position, String name);
    void DrawDamage(Unit unitDamaged, float percentHealth);
    void DrawDestroyed(Unit unit);
}
