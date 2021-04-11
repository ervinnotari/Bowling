using Bowling.Domain.Game.Entities;
using Xunit;
using System;

namespace Bowling.Domain.Game.xUnitTests
{
    public class PlayUnitTest
    {
        private const string Name = "Jos�";
        private const string Alley = "2";

        [Fact]
        public void ConstructorTest()
        {
            var date = DateTime.Now;
            var play = new Play(Name, 10, Alley, date);
            Assert.Equal(Name, play.Name);
            Assert.Equal(10, play.Pins);
            Assert.Equal(Alley, play.Alley);
            Assert.Equal(date, play.Date);
        }

        [Fact]
        public void GetsAndSetsTest()
        {
            var date = DateTime.Now;
            var play = new Play(Name, 10, Alley, date) { Name = $"_{Name}" };
            Assert.Equal($"_{Name}", play.Name);
            for (var i = 0; i <= 10; i++)
            {
                play.Pins = i;
                Assert.Equal(i, play.Pins);
            }
            play.Alley = $"_{Alley}";
            Assert.Equal($"_{Alley}", play.Alley);
            date = DateTime.Now.AddDays(-1);
            play.Date = date;
            Assert.Equal(date, play.Date);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(11)]
        [InlineData(5)]
        public void ToStringTestAndRangeException(int value)
        {
            try
            {
                var play = new Play(Name, value, Alley, DateTime.Now);
                Assert.NotNull(play);
                Assert.NotEqual("", play.ToString());
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
                Assert.IsType<ArgumentOutOfRangeException>(ex);
                Assert.NotEqual("", ex.Message);
            }
        }
    }
}