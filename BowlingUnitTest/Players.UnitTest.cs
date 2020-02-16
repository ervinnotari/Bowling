using BowlingGame.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingUnitTest
{
    [TestClass]
    public class PlayersUnitTest
    {
        private const string NAME1 = "José";
        private const string NAME2 = "Maria";
        private const string ALLEY = "01";

        [TestMethod]
        public void AddPlayTest()
        {
            var p = new Players();
            p.AddPlay(new Play(NAME1, 10, ALLEY, DateTime.Now));
            p.AddPlay(new Play(NAME1, 10, ALLEY, DateTime.Now));
            Assert.AreEqual(1, p.Count);
            p.AddPlay(new Play(NAME2, 10, ALLEY, DateTime.Now));
            Assert.AreEqual(2, p.Count);
        }
    }
}
