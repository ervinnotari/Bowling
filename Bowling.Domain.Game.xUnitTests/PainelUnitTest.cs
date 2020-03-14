using Bowling.Domain.Game.Entities;
using Xunit;
using System;

namespace Bowling.Domain.Game.xUnitTests
{
    public class PainelUnitTest
    {
        private const string Name = "Jos�";
        private const string Alley = "2";

        [Fact]
        public void GettersAndSettersTest()
        {
            var date = DateTime.Now;
            var p = new Painel(Alley, date);
            Assert.Equal(Alley, p.Alley);
            Assert.Equal(date, p.BeginGame);
            date = DateTime.Now.AddDays(-1);
            p.LastGame = date;
            Assert.Equal(date, p.LastGame);
            Assert.Empty(p.Players);
            p.Players.Add(new Player() { Name = Name });
            Assert.Single(p.Players);
        }
    }
}
