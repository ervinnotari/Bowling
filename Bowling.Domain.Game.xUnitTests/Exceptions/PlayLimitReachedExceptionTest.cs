using Bowling.Domain.Game.Exceptions;
using Xunit;
using System.Runtime.Serialization;

namespace Bowling.Domain.Game.xUnitTests.Exceptions
{
    public class PlayLimitReachedExceptionTest
    {
        [Fact]
        public void ConstructorsTest()
        {
            var cA = new PlayLimitReachedException();
            Assert.NotNull(cA);
        }

    }
}
