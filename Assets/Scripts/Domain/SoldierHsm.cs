using System;
using UnityEngine;
using Hsm;
using SoldierState = Hsm.StateWithOwner<Soldier>;

// See https://github.com/amaiorano/hsm-csharp/blob/master/samples/Sample1/Program.cs for examples

// TODO: Partial????
partial class Soldier
{
    public partial class SoldierHsm
    {
        public class Root : SoldierState
        {
            public override void OnEnter()
            {
                Debug.Log("Root state");
                SetStateValue(Owner.StateValue_HP, 100);
            }

            public override void OnExit()
            {
                Debug.Log("exit Root state");
                base.OnExit();
            }

            public override Transition GetTransition()
            {

                // TODO: Is this right?
                if (Owner.startAttack)
                {
                    Debug.Log("Go to attack");
                    return Transition.Sibling<Attack>();
                }
                return Transition.None();
            }
        }

        public class Attack : SoldierState
        {
            public override void OnEnter()
            {
                var root = GetOuterState<Root>(); // Test being able to grab Root from inner state
                Debug.Log("entering attack");
            }

            public override Transition GetTransition()
            {
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

