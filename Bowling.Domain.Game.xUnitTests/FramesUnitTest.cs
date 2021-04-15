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
        private const string Player = "Teste";
        private const string Alley = "2";

        private static void SequencialPlaysMake(Frames f, IEnumerable<int> plays)
        {
            plays.ToList().ForEach(p => f.AddPlay(new Play(Player, p, Alley, DateTime.Now)));
            Assert.NotNull(plays);
        }

        [Theory]
        [InlineData(new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 })]
        [InlineData(new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 })]
        public void GetTurnLimitTest(int[] plays)
        {
            try
            {
                var frames = new Frames();
                SequencialPlaysMake(frames, plays);
                frames.AddPlay(new Play(Player, 10, Alley, DateTime.Now));
                Assert.Equal(300, frames.GetTotal());
            }
            catch (Exception ex)
            {
                Assert.IsType<PlayLimitReachedException>(ex);
            }
        }
    }
}