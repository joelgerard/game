﻿using System;
using static State;
using static UnitGameEvents;

public class DeadState : IState
{
    readonly Unit unit;

    public DeadState(Unit unit)
    {
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    private DeadState()
    {
    }

    public  Transition GetTransition(IState state)
    {
        return null;
    }

    public  Transition Update(Unit unit, float deltaTime)
    {
        return null;
    }

    public UnitEvent GetAssociatedEvent() 
    {
        UnitDiedEvent unitDiedEvent = new UnitDiedEvent
        {
            Unit = this.GetUnit()
        };
        return unitDiedEvent; 
    }

}
