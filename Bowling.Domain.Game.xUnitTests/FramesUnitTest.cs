using Bowling.Domain.Game.Entities;
using Bowling.Domain.Game.Exceptions;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Game.xUnitTests
{
    public class FramesUnitTest
    {
        private const string Player = "Jos?";
        private const string Alley = "2";

        private static void SequencialPlaysMake(Frames f, IEnumerable<int> plays)
        {
            plays.ToList().ForEach(p => f.AddPlay(new Play(Player, p, Alley, DateTime.Now)));
        }

        [Theory]
        [InlineData(new[] {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}, 300)]
        public void GetTurnLimitTest(int[] plays, int assert)
        {
            var frames = new Frames();
            SequencialPlaysMake(frames, plays);
            Assert.Equal(assert, frames.GetTotal());
            Assert.Throws<PlayLimitReachedException>(() => frames.AddPlay(new Play(Player, 10, Alley, DateTime.Now)));
        }
    }
}