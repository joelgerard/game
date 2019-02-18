using System;
public class Diagnostics
{
    public Diagnostics()
    {
    }

    public static void NotNull(Object obj, String name)
    {
        if (obj == null)
        {
            throw new NullReferenceException(name + " is null");
        }
    }
}
