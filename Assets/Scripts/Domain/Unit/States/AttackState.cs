using System;
public class AttackState : BaseUnitState
{

    public Transition Update(State state)
    {
        // TODO: Maybe it should attack the other object?
        return null;
    }

    public Transition Exit()
    {
        return new Transition(this, TransitionType.TEMPORARY);
    }

    public override Transition GetTransition(State state)
    {
        Transition transition = base.GetTransition(state);
        return transition;
    }

    public override Transition Update(Unit unit)
    {
        Transition transition = base.Update(unit);
        if (transition != null)
        {
            return transition;
        }
        else
        {
            // throw new NotImplementedException();
            // TODO: What about actually attacking? 
            return null;
        }
    }
}
