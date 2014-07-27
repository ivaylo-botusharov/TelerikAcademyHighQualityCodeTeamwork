using System;
using BalloonsPop.UserInputOutput;
using Wintellect.PowerCollections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BalloonsPopTests
{
    [TestClass]
    public class ConsoleOutputTests
    {
        [TestMethod]
        public void ScoreboardStringCreationTest()
        {
            var statistics = new OrderedMultiDictionary<int, string>(true);
            statistics.Add(5, "Gosho");
            statistics.Add(15, "Pesho");

            var scoreboard = "Scoreboard:" + Environment.NewLine +
                "2. {Gosho} --> 5 moves" + Environment.NewLine +
                "2. {Pesho} --> 15 moves" + Environment.NewLine;

            Assert.AreEqual(scoreboard, ConsoleIOFacade.CreateScoreboardString(statistics));
        }
    }
}
