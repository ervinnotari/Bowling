using BowlingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingUnitTest
{
    [TestClass]
    public class PlayerUnitTest
    {
        private const string NAME = "José";

        [TestMethod]
        public void GettersAndSettersTest()
        {
            var p = new Player
            {
                Name = NAME
            };
            Assert.AreEqual(NAME, p.Name);
            Assert.AreEqual(0, p.Frames.Count);
            p.Frames.Add(new Frame(null));
            Assert.AreEqual(1, p.Frames.Count);
        }
    }
}
