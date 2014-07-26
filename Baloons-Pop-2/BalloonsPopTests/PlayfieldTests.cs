namespace BalloonsPopTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BalloonsPop;

    [TestClass]
    public class PlayfieldTests
    {
        [TestMethod]
        public void InitialHeightShouldBeTenForNewInstance()
        {
            Playfield playfield = new Playfield();
            int expected = 10;
            int actual = playfield.Height;

            Assert.AreEqual(expected, actual, "When new instance is created without parameters, the height of the playfield should have value of 10.");
        }

        [TestMethod]
        public void InitialWidthShouldBeFiveForNewInstance()
        {
            Playfield playfield = new Playfield();
            int expected = 5;
            int actual = playfield.Width;

            Assert.AreEqual(expected, actual, "When new instance is created without parameters, the width of the playfield should have value of 5.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeHeightShouldThrowException()
        {
            Playfield playfield = new Playfield(-20);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeWidthShouldThrowException()
        {
            Playfield playfield = new Playfield(10, -20);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ZeroHeightShouldThrowException()
        {
            Playfield playfield = new Playfield(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ZeroWidthShouldThrowException()
        {
            Playfield playfield = new Playfield(10, 0);
        }
    }
}
