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
