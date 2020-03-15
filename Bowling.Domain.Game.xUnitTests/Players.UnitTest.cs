using Bowling.Domain.Game.Entities;
using Xunit;
using System;

namespace Bowling.Domain.Game.xUnitTests
{
    public class PlayersUnitTest
    {
        private const string Name1 = "Jos�";
        private const string Name2 = "Maria";
        private const string Alley = "01";

        [Fact]
        public void AddPlayTest()
        {
            var p = new Players();
            p.AddPlay(new Play(Name1, 10, Alley, DateTime.Now));
            p.AddPlay(new Play(Name1, 10, Alley, DateTime.Now));
            Assert.Single(p);
            p.AddPlay(new Play(Name2, 10, Alley, DateTime.Now));
            Assert.Equal(2, p.Count);
        }
    }
}
