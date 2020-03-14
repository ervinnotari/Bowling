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
            var play = new Play(Name, 10, Alley, date) {Name = $"_{Name}"};
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


        [Fact]
        public void PinsLess0ExceptionTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Play(Name, -1, Alley, DateTime.Now));
        }

        [Fact]
        public void PinsMore10ExceptionTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Play(Name, 11, Alley, DateTime.Now));
        }

        [Fact]
        public void ToStringTest()
        {
            var play = new Play(Name, 5, Alley, DateTime.Now);
            Assert.NotEqual("", play.ToString());
        }
    }
}