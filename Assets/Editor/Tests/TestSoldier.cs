using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture()]
    public class TestSoldier
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestSoldierSimplePasses()
        {
            GameObject go = new GameObject();
            Vector2 newPos = new Vector2(1,2);
            //tr.AddPosition(newPos);
            //tr.SetPosition(0, newPos);
            //Vector2 position = tr.GetPosition(0);
            //Assert.NotNull(position);
            // Use the Assert class to test conditions
            Assert.True(1f == newPos.x);

        }

        [Test]
        public void TestBattle()
        {
            //Game game = new Game();
            //game.Initialize();

            ArmyBase armyBase = new ArmyBase
            {
                Allegiance = Allegiance.ENEMY
            };
            armyBase.Init();

            Soldier soldier = new Soldier
            {
                Allegiance = Allegiance.ALLY
            };
            soldier.Init();

            // Moves into neutral.
            // TODO: Why do I have this state.
            soldier.Update(1);
            armyBase.Update(1);
            Assert.True(soldier.StateMachine.IsInState<Unit.UnitHsm.Neutral>());

            soldier.Attack(armyBase);

            // Moves into attack
            soldier.Update(1);
            armyBase.Update(1);

            Unit.UnitHsm.Attack attack = new Unit.UnitHsm.Attack();

            MovableUnit.MovableUnitHsm.Attack state = soldier.StateMachine.FindState<MovableUnit.MovableUnitHsm.Attack>(); //<Unit.UnitHsm.Attack>();
            Assert.NotNull(state);

            //Assert.True(soldier.StateMachine.IsInState<Unit.UnitHsm.Attack>());

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        /*[UnityTest]
        public IEnumerator TestSoldierWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }*/
    }
}
