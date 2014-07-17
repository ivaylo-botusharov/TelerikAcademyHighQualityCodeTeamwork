namespace BalloonsPopTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BalloonsPop;

    [TestClass]
    public class RecursivePopStrategyTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallingPopBalloonsWithNullPlayFieldReferenceShouldThrowAnException()
        {
            RecursivePopStrategy popStrategy = new RecursivePopStrategy();

            popStrategy.PopBaloons(1, 1, "1", null);
        }
    }
}
