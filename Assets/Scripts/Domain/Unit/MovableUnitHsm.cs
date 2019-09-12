using System;
using Hsm;
using MovableUnitState = Hsm.StateWithOwner<MovableUnit>;

public partial class MovableUnit
{
    public partial class MovableUnitHsm : UnitHsm
    {
        // TODO: This isn't right, because something can set the 
        // state back to "Root", which is in the base class.
        public class MovableNeutral : Unit.UnitHsm.Neutral
        {
            public override void OnEnter()
            {
                // TODO: What do I do about this? Use it here? 
                //SetStateValue(Owner.StateValue_HP, 100);
            }

            public override void OnExit()
            {
                base.OnExit();
            }

            public override Transition GetTransition()
            {
                Transition transition = base.GetTransition();
                if (TransitionType.None != Transition.None().TransitionType)
                {
                    return transition;
                }

                if (((MovableUnit)Owner).startMoving)
                {
                    GameController.Log("Start moving");
                    return Transition.Sibling<Moving>();
                }

                return base.GetTransition();
            }
        }



        public class Moving : MovableUnitState
        {
            public override void OnEnter()
            {
                GameController.Log("Starting to move");
                //var root = GetOuterState<Root>(); // Test being able to grab Root from inner state
                //Debug.Log("entering attack");
            }

            public override void Update(float aDeltaTime)
            {
                /*MovableUnit unit = (MovableUnit)Owner;
                Navigator navigator = new Navigator();
                navigator.MoveUnits(null,unit.Path);*/
                base.Update(aDeltaTime);
            }

            public override Transition GetTransition()
            {
                if (Owner.startAttack)
                {
                    return Transition.Sibling<Unit.UnitHsm.Attack>();
                }
                //Debug.Log("In Attack.GetTrans");
                /*if (FindInnerState<Driving>() != null)
                {
                    Console.Out.WriteLine("Healthy state: my inner is Driving!");
                }*/

                return Transition.None(); //Transition.InnerEntry<Platforming>("Yo!");
            }
        }
    }
}
