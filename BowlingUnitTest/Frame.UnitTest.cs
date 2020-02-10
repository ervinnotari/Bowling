using BowlingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingUnitTest
{
    [TestClass]
    public class FrameUnitTest
    {
        private const string NAME = "José";

        [TestMethod]
        public void GettersAndSettersTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(1);
            f1.Balls.Add(4);
            Assert.AreEqual(5, f1.Score);
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.AreEqual(15, f2.Score);
        }

        [TestMethod]
        public void IsSpareTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(1);
            f1.Balls.Add(4);
            Assert.IsFalse(f1.IsSpare());
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.IsTrue(f2.IsSpare());
        }

        [TestMethod]
        public void IsOpenTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(1);
            f1.Balls.Add(4);
            Assert.IsFalse(f1.IsOpen());
            var f2 = new Frame(f1);
            f2.Balls.Add(10);
            Assert.IsFalse(f2.IsOpen());
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(5);
            Assert.IsFalse(f3.IsOpen());
            var f4 = new Frame(f3);
            f4.Balls.Add(5);
            Assert.IsTrue(f4.IsOpen());
        }

        [TestMethod]
        public void IsStrikeTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(10);
            Assert.IsTrue(f1.IsStrike());
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.IsFalse(f2.IsStrike());
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(1);
            Assert.IsFalse(f2.IsStrike());
        }

        [TestMethod]
        public void GetAttemptTest()
        {
            var f0 = new Frame(null);
            Assert.AreEqual(0, f0.GetAttempt());
            var f1 = new Frame(f0);
            f1.Balls.Add(10);
            Assert.AreEqual(1, f1.GetAttempt());
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.AreEqual(2, f2.GetAttempt());
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(1);
            f3.Balls.Add(10);
            Assert.AreEqual(3, f3.GetAttempt());
        }

        [TestMethod]
        public void ScoreTest()
        {
            var f0 = new Frame(null);
            Assert.AreEqual(0, f0.Score);
            var f1 = new Frame(f0);
            f1.Balls.Add(10);
            Assert.AreEqual(10, f1.Score);
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.AreEqual(20, f2.Score);
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(1);
            f3.Balls.Add(10);
            Assert.AreEqual(36, f3.Score);
        }
    }
}
