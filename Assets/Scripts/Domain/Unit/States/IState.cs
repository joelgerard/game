using System;
using static State;

public interface IState
{
    Transition Update(Unit unit, float deltaTime);
    Transition GetTransition(IState state);
    UnitEvent GetAssociatedEvent();
}
