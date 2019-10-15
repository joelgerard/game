using System;

// I just can't figure out a good fucking name for this thing.
// And I don't think it should inherit from Unit or unit should be modified.
public class Thing : Unit
{
    public enum ThingTypeEnum
    {
        PathOrigin
    }

    public ThingTypeEnum ThingType { get; set; }

    public Thing()
    {
    }



}
