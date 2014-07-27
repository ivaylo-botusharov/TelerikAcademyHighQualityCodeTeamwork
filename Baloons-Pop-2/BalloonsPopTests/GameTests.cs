using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BalloonsPop;

namespace BalloonsPopTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void GameInstanceTest()
        {
            Game testGame = Game.Instance;

            Assert.IsTrue(testGame is Game);
        }
    }
}
