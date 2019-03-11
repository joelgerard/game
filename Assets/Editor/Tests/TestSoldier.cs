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

        [Test]
        public void StandingUnitsCanAttack()
        {
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
            soldier.Attack(armyBase);
            soldier.Update(1);
            Assert.True(soldier.StateMachine.IsInState<Unit.UnitHsm.Attack>());
        }

        [Test]
        public void MovingUnitsCanAttack()
        {
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
            soldier.StartMoving();
            soldier.Attack(armyBase);
            soldier.Update(1);
            Assert.True(soldier.StateMachine.IsInState<Unit.UnitHsm.Attack>());
        }

        [Test]
        public void UnitsDontAutoCounterAttack()
        {
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
            soldier.StartMoving();
            soldier.Attack(armyBase);
            soldier.Update(1);
            Assert.False(armyBase.StateMachine.IsInState<Unit.UnitHsm.Attack>());
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
