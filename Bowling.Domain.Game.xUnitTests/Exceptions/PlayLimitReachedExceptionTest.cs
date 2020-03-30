using System;
using System.Runtime.Serialization;
using Bowling.Domain.Game.Exceptions;
using Xunit;

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
