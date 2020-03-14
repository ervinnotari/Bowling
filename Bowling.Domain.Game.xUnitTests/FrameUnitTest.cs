using Bowling.Domain.Game.Entities;
using Xunit;

namespace Bowling.Domain.Game.xUnitTests
{
    public class FrameUnitTest
    {
        private const string Name = "Jos�";

        [Fact]
        public void GettersAndSettersTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(1);
            f1.Balls.Add(4);
            Assert.Equal(5, f1.Score);
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.Equal(15, f2.Score);
        }

        [Fact]
        public void IsSpareTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(1);
            f1.Balls.Add(4);
            Assert.False(f1.IsSpare());
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.True(f2.IsSpare());
        }

        [Fact]
        public void IsOpenTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(1);
            f1.Balls.Add(4);
            Assert.False(f1.IsOpen());
            var f2 = new Frame(f1);
            f2.Balls.Add(10);
            Assert.False(f2.IsOpen());
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(5);
            Assert.False(f3.IsOpen());
            var f4 = new Frame(f3);
            f4.Balls.Add(5);
            Assert.True(f4.IsOpen());
        }

        [Fact]
        public void IsStrikeTest()
        {
            var f1 = new Frame(null);
            f1.Balls.Add(10);
            Assert.True(f1.IsStrike());
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.False(f2.IsStrike());
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(1);
            Assert.False(f2.IsStrike());
        }

        [Fact]
        public void GetAttemptTest()
        {
            var f0 = new Frame(null);
            Assert.Equal(0, f0.GetAttempt());
            var f1 = new Frame(f0);
            f1.Balls.Add(10);
            Assert.Equal(1, f1.GetAttempt());
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.Equal(2, f2.GetAttempt());
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(1);
            f3.Balls.Add(10);
            Assert.Equal(3, f3.GetAttempt());
        }

        [Fact]
        public void ScoreTest()
        {
            var f0 = new Frame(null);
            Assert.Equal(0, f0.Score);
            var f1 = new Frame(f0);
            f1.Balls.Add(10);
            Assert.Equal(10, f1.Score);
            var f2 = new Frame(f1);
            f2.Balls.Add(5);
            f2.Balls.Add(5);
            Assert.Equal(20, f2.Score);
            var f3 = new Frame(f2);
            f3.Balls.Add(5);
            f3.Balls.Add(1);
            f3.Balls.Add(10);
            Assert.Equal(36, f3.Score);
        }
    }
}
