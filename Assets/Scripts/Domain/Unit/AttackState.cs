using System;
public class AttackState : UnitState
{

    public Transition Update(State state)
    {
        // TODO: Only state to go here is death.
        return null;
    }

    public Transition Exit()
    {
        return new Transition(this, TransitionType.TEMPORARY);
    }

    public override Transition GetTransition(State state)
    {
        throw new NotImplementedException();
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
            //throw new NotImplementedException();
            // TODO: What about actually attacking? 
            return null;
        }
    }
}
