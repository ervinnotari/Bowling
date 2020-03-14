using Bowling.Domain.Game.Entities;
using Xunit;

namespace Bowling.Domain.Game.xUnitTests
{
    public class PlayerUnitTest
    {
        private const string Name = "Jos�";

        [Fact]
        public void GettersAndSettersTest()
        {
            var p = new Player
            {
                Name = Name
            };
            Assert.Equal(Name, p.Name);
            Assert.Empty(p.Frames);
            p.Frames.Add(new Frame(null));
            Assert.Single(p.Frames);
        }
    }
}
