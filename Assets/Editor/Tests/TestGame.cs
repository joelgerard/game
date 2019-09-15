using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture()]
    public class TestGame
    {

        [Test]
        public void UnitsDontAutoCounterAttack()
        {
            // TODO: Replace.

            //Game game = new Game();
            //game.Initialize();

            //GameObject go = new GameObject
            //{
            //    name = "Soldier1"
            //};
            //Soldier s = game.OnAddSoldier(go,Allegiance.ALLY);
            //s.StartMoving();
            //game.OnUnitsCollide("Soldier1", "EnemyBaseSquare");
            //GameUpdate gu = new GameUpdate()
            //{
            //    deltaTime=1,
            //    currentPath=null
            //};


            //game.Update(gu);

            //Assert.True(s.StateMachine.IsInState<Unit.UnitHsm.Attack>());
            //Assert.False(game.Enemy.ArmyBase.StateMachine.IsInState<Unit.UnitHsm.Attack>());
            Assert.True(false);
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
