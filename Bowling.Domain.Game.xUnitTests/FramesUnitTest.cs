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

        [Fact]
        public void GetTurnLimitTest()
        {
            var frames = new Frames();
            SequencialPlaysMake(frames, new[] { 10, 10, 10, 10, 10 });
            SequencialPlaysMake(frames, new[] { 10, 10, 10, 10, 10 });
            SequencialPlaysMake(frames, new[] { 10, 10 });
            Assert.Equal(300, frames.GetTotal());
            Assert.Throws<PlayLimitReachedException>(() => frames.AddPlay(new Play(Player, 10, Alley, DateTime.Now)));
        }
    }
}