using System;
using Hsm;
using UnitState = Hsm.StateWithOwner<Unit>;

// See https://github.com/amaiorano/hsm-csharp/blob/master/samples/Sample1/Program.cs for examples
// Better docs at the original c++
// https://github.com/amaiorano/hsm/wiki/Chapter-4.-Advanced-Techniques#sharing-functions-across-states


public partial class Unit
{
    public partial class UnitHsm
    {
        public class Root : UnitState
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
                return Owner.GetNeutralTransition(); 
            }
        }

        public class Neutral : UnitState
        {
            public override void OnEnter()
            {

            }

            public override void OnExit()
            {

            }

            public override Transition GetTransition()
            {
                if (Owner.startAttack)
                {
                    return Transition.Sibling<Attack>();
                }

                return Transition.None();
            }
        }

        public class Attack : UnitState
        {
            public override void OnEnter()
            {
                var root = GetOuterState<Root>(); // Test being able to grab Root from inner state
            }

            public override void Update(float aDeltaTime)
            {
                // TODO: Should this use state values?
                Owner.startAttack = Owner.enemy.Damage(1);
                base.Update(aDeltaTime);
            }

            public override Transition GetTransition()
            {
                if (Owner.enemy == null || Owner.enemy.HP <= 0)
                {
                    // TODO: This doesn't seem right. What if you are
                    // moving and fighting?
                    return Owner.GetNeutralTransition();  //Transition.Sibling<Neutral>();
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
