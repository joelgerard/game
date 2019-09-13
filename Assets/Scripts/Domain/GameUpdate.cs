using System;
using System.Collections.Generic;

public class GameUpdate
{
    public float deltaTime;
    public IPath currentPath;
    public List<GameEvent> GameEvents { get; set; } = new List<GameEvent>();
}