using Bowling.Domain.Game.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingUnitTest
{
    [TestClass]
    public class PlayUnitTest
    {
        private const string NAME = "José";
        private const string ALLEY = "2";

        [TestMethod]
        public void ConstructorTest()
        {
            var date = DateTime.Now;
            var play = new Play(NAME, 10, ALLEY, date);
            Assert.AreEqual(NAME, play.Name);
            Assert.AreEqual(10, play.Pins);
            Assert.AreEqual(ALLEY, play.Alley);
            Assert.AreEqual(date, play.Date);
        }

        [TestMethod]
        public void GetsAndSetsTest()
        {
            var date = DateTime.Now;
            var play = new Play(NAME, 10, ALLEY, date);
            play.Name = $"_{NAME}";
            Assert.AreEqual($"_{NAME}", play.Name);
            for (int i = 0; i <= 10; i++)
            {
                play.Pins = i;
                Assert.AreEqual(i, play.Pins);
            }
            play.Alley = $"_{ALLEY}";
            Assert.AreEqual($"_{ALLEY}", play.Alley);
            date = DateTime.Now.AddDays(-1);
            play.Date = date;
            Assert.AreEqual(date, play.Date);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PinsLess0ExceptionTest()
        {
            var play = new Play(NAME, -1, ALLEY, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PinsMore10ExceptionTest()
        {
            var play = new Play(NAME, 11, ALLEY, DateTime.Now);
        }

        [TestMethod]
        public void ToStringTest()
        {
            var play = new Play(NAME, 5, ALLEY, DateTime.Now);
            Assert.IsInstanceOfType(play.ToString(), typeof(string));
        }
    }
}