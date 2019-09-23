using System;
using System.Collections.Generic;

public class GameUpdate
{
    public float deltaTime;
    public IPath currentPath;
    public List<UnityGameEvent> GameEvents { get; set; } = new List<UnityGameEvent>();
}