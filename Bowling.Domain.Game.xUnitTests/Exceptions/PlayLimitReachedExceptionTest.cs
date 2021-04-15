using Bowling.Domain.Game.Exceptions;
using Xunit;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bowling.Domain.Game.xUnitTests.Exceptions
{
    public class PlayLimitReachedExceptionTest
    {
        [Fact]
        public void ConstructorsTest()
        {
            var exBase = new PlayLimitReachedException();
            var exResult = Record.ExceptionAsync(() =>
            {
                using Stream s = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, exBase);
                s.Position = 0;
                throw (PlayLimitReachedException)formatter.Deserialize(s);
            }).GetAwaiter().GetResult();
            Assert.NotNull(exResult);
            Assert.IsType<PlayLimitReachedException>(exResult);
        }

    }
}
