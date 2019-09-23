using System;
public class DistinctNumGenerator
{
    int i = 0;

    private DistinctNumGenerator()
    {
    }

    public static DistinctNumGenerator Instance { get; } = new DistinctNumGenerator();

    public int GetNum()
    {
        return i++;
    }
}
