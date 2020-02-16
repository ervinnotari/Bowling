using BowlingGame.Entities;
using BowlingGame.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingUnitTest
{
    [TestClass]
    public class FramesUnitTest
    {
        private const string PLAYER = "José";
        private const string ALLEY = "2";

        private static void SequencialPlaysMake(Frames f, int[] plays)
        {
            plays.ToList().ForEach(p => f.AddPlay(new Play(PLAYER, p, ALLEY, DateTime.Now)));
        }

        [TestMethod]
        [ExpectedException(typeof(PlayLimitReachedException))]
        public void GetTurnLimitTest()
        {
            var frames = new Frames();
            SequencialPlaysMake(frames, new int[] { 10, 10, 10, 10, 10 });
            SequencialPlaysMake(frames, new int[] { 10, 10, 10, 10, 10 });
            SequencialPlaysMake(frames, new int[] { 10, 10 });
            Assert.AreEqual(300, frames.GetTotal());
            frames.AddPlay(new Play(PLAYER, 10, ALLEY, DateTime.Now));
        }
    }
}
