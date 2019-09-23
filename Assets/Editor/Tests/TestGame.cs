using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static UnitGameEvents;

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


            GameUpdate gu = new GameUpdate()
            {
                deltaTime=1,
                currentPath=null
            };
            gu.UnityGameEvents.Add(new UnitsCollideEvent("Soldier1", "EnemyBaseSquare"));

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

            GameUpdate gu = new GameUpdate()
            {
                deltaTime = 0.01f,
                currentPath = null
            };
            gu.UnityGameEvents.Add(new UnitsCollideEvent("Soldier1", "EnemyBaseSquare"));
            gu.UnityGameEvents.Add(new UnitsCollideEvent("EnemyBaseSquare", "Soldier1"));
            game.Update(gu);


            Assert.True(s.StateMachine.IsInState<AttackState>());
            Assert.True(game.Enemy.ArmyBase.StateMachine.IsInState<AttackState>());

        }

        [Test]
        public void MultipleAttackersDie()
        {
            Game game = new Game();
            FakeGameService gameService = new FakeGameService(game);

            game.Initialize();
            gameService.RenderUnits(game.DrawMap());


            Soldier s1 = game.AddSoldier(Allegiance.ALLY, new Vector2(1f, 1f));
            s1.StartMoving();
            s1.Name = "Soldier1";
            gameService.RenderUnit(s1);

            Soldier s2 = game.AddSoldier(Allegiance.ALLY, new Vector2(1f, 1f));
            s2.StartMoving();
            s2.Name = "Soldier2";
            gameService.RenderUnit(s2);

            GameUpdate gu = new GameUpdate()
            {
                deltaTime = 1,
                currentPath = null
            };
            gu.UnityGameEvents.Add(new UnitsCollideEvent("Soldier1", "EnemyBaseSquare"));
            gu.UnityGameEvents.Add(new UnitsCollideEvent("Soldier2", "EnemyBaseSquare"));

            int i = 0;
            while (true)
            {
                game.Update(gu);
                if (s1.StateMachine.IsInState<DyingState>())
                {
                    gu.UnityGameEvents.Add(new UnitExplosionComplete("Soldier1"));
                }
                if (s2.StateMachine.IsInState<DyingState>())
                {
                    gu.UnityGameEvents.Add(new UnitExplosionComplete("Soldier2"));
                }
                if (s1.StateMachine.IsInState<DeadState>() &&
                   s2.StateMachine.IsInState<DeadState>()
                )
                {
                    break;
                }

                i++;
                if (i == 100)
                {
                    Assert.Fail("Timeout");
                }
            }
            game.Update(gu);
            Assert.Pass();

            //Assert.True(s1.StateMachine.IsInState<AttackState>());
            //Assert.False(game.Enemy.ArmyBase.StateMachine.IsInState<AttackState>());

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
