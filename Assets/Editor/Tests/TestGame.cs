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
            Game game = new Game();
            FakeGameService gameService = new FakeGameService(game);

            game.Initialize();
            gameService.RenderUnits(game.DrawMap());


            Soldier s = game.AddSoldier(Allegiance.ALLY, new Vector2(1f, 1f));
            s.StartMoving();
            s.Name = "Soldier1";
            gameService.RenderUnit(s);

            game.OnUnitsCollide("Soldier1", "EnemyBaseSquare");

            GameUpdate gu = new GameUpdate()
            {
                deltaTime=1,
                currentPath=null
            };

            game.Update(gu);


            Assert.True(s.StateMachine.IsInState<AttackState>());
            Assert.False(game.Enemy.ArmyBase.StateMachine.IsInState<AttackState>());

        }

        [Test]
        public void BaseAttacks()
        {
            Game game = new Game();
            FakeGameService gameService = new FakeGameService(game);

            game.Initialize();
            gameService.RenderUnits(game.DrawMap());


            Soldier s = game.AddSoldier(Allegiance.ALLY, new Vector2(1f, 1f));
            s.StartMoving();
            s.Name = "Soldier1";
            gameService.RenderUnit(s);

            game.OnUnitsCollide("Soldier1", "EnemyBaseSquare");
            game.OnUnitsCollide("EnemyBaseSquare", "Soldier1");

            GameUpdate gu = new GameUpdate()
            {
                deltaTime = 0.01f,
                currentPath = null
            };

            game.Update(gu);


            Assert.True(s.StateMachine.IsInState<AttackState>());
            Assert.True(game.Enemy.ArmyBase.StateMachine.IsInState<AttackState>());

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
