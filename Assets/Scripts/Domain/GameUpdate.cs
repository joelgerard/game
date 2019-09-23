using System;
using System.Collections.Generic;

public class GameUpdate
{
    public float deltaTime;
    public IPath currentPath;
    public List<UnityGameEvent> UnityGameEvents { get; set; } = new List<UnityGameEvent>();
}