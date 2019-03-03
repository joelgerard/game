﻿using System;
using Hsm;
using static MovableUnit.MovableUnitHsm;

public partial class MovableUnit : Unit
{
    protected bool startMoving = false;

    public partial class MovableUnitHsm { }

    public MovableUnit()
    {
    }

    public void StartMoving()
    {
        startMoving = true;
    }

    /** ############ SHARED STATES ########### */
    // C# HSM doesn't yet support shared states.
    protected override Transition GetNeutralTransition()
    {
        return Transition.Sibling<MovableNeutral>();
    }
}