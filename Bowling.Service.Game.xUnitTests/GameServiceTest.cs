using System;
using System.Threading.Tasks;
using Bowling.Domain.Game.Entities;
using Xunit;

namespace Bowling.Service.Game.xUnitTests
{
    public class GameServiceTest
    {
        [Fact]
        public void AddPlayTest()
        {
            var t = new GameService();
            const string alley = "1";
            Assert.False(t.IsExistsAlley(alley));
            t.AddPlay(new Play("teste", 10, alley, DateTime.Now));
            Assert.True(t.IsExistsAlley(alley));
        }

        [Fact]
        public void ClearTest()
        {
            var t = new GameService();
            const string alley = "1";
            Assert.False(t.IsExistsAlley(alley));
            t.AddPlay(new Play("teste", 10, alley, DateTime.Now));
            Assert.True(t.IsExistsAlley(alley));
            t.Clear(alley);
            Assert.Empty(t.GetPainel(alley).Players);
        }

        [Fact]
        public void GetPainelTest()
        {
            var t = new GameService();
            const string alley = "1";
            Assert.Null(t.GetPainel(alley));
            t.AddPlay(new Play("teste", 10, alley, DateTime.Now));
            var rst = t.GetPainel(alley);
            Assert.NotNull(rst);
            Assert.IsType<Painel>(rst);
        }

        [Fact]
        public void GetAlleysNameTest()
        {
            var t = new GameService();
            Assert.Empty(t.GetAlleysName());
            t.AddPlay(new Play("teste", 10, "1", DateTime.Now));
            Assert.Single(t.GetAlleysName());
            t.AddPlay(new Play("teste", 10, "2", DateTime.Now));
            Assert.Equal(2, t.GetAlleysName().Count);

        }

        [Fact]
        public void GetScoreAsyncTest()
        {
            Task.Run(async () =>
            {
                var t = new GameService();
                const string alley = "1";
                Assert.Null(await t.GetScoreAsync(alley));
                t.AddPlay(new Play("teste", 10, alley, DateTime.Now));
                var rst = await t.GetScoreAsync(alley);
                Assert.NotNull(rst);
                Assert.IsType<Painel>(rst);
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void EventsTest()
        {
            Task.Run(async () =>
            {
                var t = new GameService();
                t.OnPlay += EventOnPlay;
                t.OnChange += EventOnChange;
                const string alley = "1";
                Assert.Null(await t.GetScoreAsync(alley));
                t.AddPlay(new Play("teste", 10, alley, DateTime.Now));
                var rst = await t.GetScoreAsync(alley);
                Assert.NotNull(rst);
                Assert.IsType<Painel>(rst);
                t.Clear(alley);
                Assert.Empty(rst.Players);
            }).GetAwaiter().GetResult();
        }

        private void EventOnChange(object obj) => Assert.NotNull(obj);

        private Play EventOnPlay(Play p)
        {
            Assert.NotNull(p);
            return p;
        }
    }
}
