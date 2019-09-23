﻿using System;
using static State;

public class MovingState : IState
{
    private MovingState()
    {

    }

    readonly Unit unit;

    public MovingState(Unit unit)
    {
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }


    public Transition GetTransition(IState state)
    {
        return (new SharedStatesTransitioner()).GetTransition(state, this);
    }

    public Transition Update(Unit unit, float deltaTime)
    {
        return (new SharedStatesTransitioner()).Update(this, deltaTime);
    }

    public UnitEvent GetAssociatedEvent() { return null; }
}
