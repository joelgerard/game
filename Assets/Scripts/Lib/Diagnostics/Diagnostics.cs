using System;
using System.Collections;
using System.Collections.Generic;

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

    public static void KeyExists(IDictionary dictionary, object key)
    {
        if (!dictionary.Contains(key))
        {
            throw new KeyNotFoundException("Key not found " + key.ToString());
        }
    }
}
