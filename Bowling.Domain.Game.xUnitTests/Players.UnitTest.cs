using Bowling.Domain.Game.Entities;
using Xunit;
using System;

namespace Bowling.Domain.Game.xUnitTests
{
    public class PlayersUnitTest
    {
        private const string NAME1 = "Jos�";
        private const string NAME2 = "Maria";
        private const string ALLEY = "01";

        [Fact]
        public void AddPlayTest()
        {
            var p = new Players();
            p.AddPlay(new Play(NAME1, 10, ALLEY, DateTime.Now));
            p.AddPlay(new Play(NAME1, 10, ALLEY, DateTime.Now));
            Assert.Single(p);
            p.AddPlay(new Play(NAME2, 10, ALLEY, DateTime.Now));
            Assert.Equal(2, p.Count);
        }
    }
}
