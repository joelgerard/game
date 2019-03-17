using System;
using UnityEngine;

public interface IUnitRenderer
{
    MovingObject Draw(Vector2 position);
    MovingObject Draw(Vector2 position, String name);
    void DrawDamage(Unit unitDamaged, float percentHealth);
    void DrawDestroyed(Unit unit);
}
