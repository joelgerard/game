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

            soldier.Attack(armyBase);
            soldier.Update(0.1f);
            Assert.True(soldier.StateMachine.IsInState<AttackState>());
        }

        [Test]
        public void MovingUnitsCanAttack()
        {
            SoldierBase sBase = new SoldierBase();
            ArmyBase armyBase = sBase.armyBase;
            Soldier soldier = sBase.soldier;

            soldier.StartMoving();
            soldier.Update(1);
            Assert.True(soldier.StateMachine.IsInState<MovingState>());

            soldier.Attack(armyBase);
            soldier.Update(1);

            Assert.True(soldier.StateMachine.IsInState<AttackState>());
        }

        [Test]
        public void UnitsDontAutoCounterAttack()
        {
            SoldierBase sBase = new SoldierBase();
            ArmyBase armyBase = sBase.armyBase;
            Soldier soldier = sBase.soldier;

            soldier.Attack(armyBase);
            soldier.Update(1);
            Assert.False(armyBase.StateMachine.IsInState<AttackState>());
        }

        [Test]
        public void UnitsAttackAndOneDies()
        {
            Soldier ally = new Soldier
            {
                Allegiance = Allegiance.ALLY
            };
            Soldier enemy = new Soldier()
            {
                Allegiance = Allegiance.ENEMY
            };
            ally.Init();
            enemy.Init();

            int timeout = 100;

            while (enemy.HP > 0)
            {
                ally.Attack(enemy);
                enemy.Attack(ally);
                ally.Update(1);
                enemy.Update(1);
                timeout--;
                if (timeout == 0)
                {
                    Assert.Fail("Timeout");
                    break;
                }
            }
            Assert.True(enemy.StateMachine.IsInState<DeadState>());

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
