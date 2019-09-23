using System;
using static State;

public interface IState
{
    Unit GetUnit();
    Transition Update(Unit unit, float deltaTime);
    Transition GetTransition(IState state);

    // TODO: Is this used? If so, how?
    UnitEvent GetAssociatedEvent();
}
