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
        class SoldierBase
        {
            public ArmyBase armyBase = new ArmyBase
            {
                Allegiance = Allegiance.ENEMY
            };
            public Soldier soldier = new Soldier
            {
                Allegiance = Allegiance.ALLY
            };

            public SoldierBase()
            {
                armyBase.Init();
                soldier.Init();
            }
        }

        [Test]
        public void StandingUnitsCanAttack()
        {
            SoldierBase sBase = new SoldierBase();
            ArmyBase armyBase = sBase.armyBase;
            Soldier soldier = sBase.soldier;

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
            SoldierBase sBase = new SoldierBase();
            ArmyBase armyBase = sBase.armyBase;
            Soldier soldier = sBase.soldier;

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
            SoldierBase sBase = new SoldierBase();
            ArmyBase armyBase = sBase.armyBase;
            Soldier soldier = sBase.soldier;

            // Moves into neutral.
            // TODO: Why do I have this state.
            soldier.Update(1);
            soldier.StartMoving();
            soldier.Attack(armyBase);
            soldier.Update(1);
            Assert.False(armyBase.StateMachine.IsInState<Unit.UnitHsm.Attack>());
        }

        [Test]
        public void UnitsAttackAndOneDies()
        {
            Assert.True(false);
            //Soldier ally = new Soldier
            //{
            //    Allegiance = Allegiance.ALLY
            //};
            //Soldier enemy = new Soldier()
            //{
            //    Allegiance = Allegiance.ENEMY
            //};
            //ally.Init();
            //enemy.Init();

            //while (enemy.HP > 0)
            //{
            //    ally.Attack(enemy);
            //    enemy.Attack(ally);
            //}


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
