using Bowling.Domain.Game.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BowlingUnitTest
{
    [TestClass]
    public class PainelUnitTest
    {
        private const string NAME = "José";
        private const string ALLEY = "2";

        [TestMethod]
        public void GettersAndSettersTest()
        {
            var date = DateTime.Now;
            var p = new Painel(ALLEY, date);
            Assert.AreEqual(ALLEY, p.Alley);
            Assert.AreEqual(date, p.BeginGame);
            date = DateTime.Now.AddDays(-1);
            p.LastGame = date;
            Assert.AreEqual(date, p.LastGame);
            Assert.AreEqual(0, p.Players.Count);
            p.Players.Add(new Player() { Name = NAME });
            Assert.AreEqual(1, p.Players.Count);
        }
    }
}
