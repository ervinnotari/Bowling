using System;
using System.Collections.Generic;
using Bowling.Domain.Game.Entities;
using Xunit;
using System.Linq;
using Bowling.Domain.Game.Exceptions;

namespace Bowling.Domain.Game.xUnitTests
{
    public class BowlingUnitTest
    {
        private const string Player = "Jos�";
        private const string Alley = "2";

        private static void SequencialPlaysMake(Game.Entities.Game b, IEnumerable<int> plays)
        {
            plays.ToList().ForEach(p => { b.AddPlay(new Play(Player, p, Alley, DateTime.Now)); });
        }

        [Theory]
        [InlineData(new[] {1, 4}, 5, 2)]
        [InlineData(new[] {1, 4, 4, 5}, 14, 4)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4}, 24, 6)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5}, 39, 8)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10}, 59, 9)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1}, 61, 11)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3}, 71, 13)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4}, 87, 15)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10}, 107, 16)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8}, 127, 18)]
        [InlineData(new[] {1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6}, 133, 19)]
        [InlineData(new[] {10}, 10, 1)]
        [InlineData(new[] {10, 10}, 30, 2)]
        [InlineData(new[] {10, 10, 10}, 60, 3)]
        [InlineData(new[] {10, 10, 10, 10}, 90, 4)]
        [InlineData(new[] {10, 10, 10, 10, 10}, 120, 5)]
        [InlineData(new[] {10, 10, 10, 10, 10, 10}, 150, 6)]
        [InlineData(new[] {10, 10, 10, 10, 10, 10, 10}, 180, 7)]
        [InlineData(new[] {10, 10, 10, 10, 10, 10, 10, 10}, 210, 8)]
        [InlineData(new[] {10, 10, 10, 10, 10, 10, 10, 10, 10}, 240, 9)]
        [InlineData(new[] {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}, 290, 11)]
        [InlineData(new[] {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}, 300, 12)]
        [InlineData(new[] {10, 10, 8, 2, 8, 2, 9, 1, 10, 8, 1, 9, 0, 8, 1, 10, 9, 1}, 171, 18)]
        [InlineData(new[] {9, 1, 8, 2, 9, 1, 6, 3, 8, 2, 10, 10, 10, 10, 10, 9, 1}, 221, 17)]
        [InlineData(new[] {10, 9, 1, 8, 2, 10, 7, 1, 10, 8, 2, 10, 10, 10, 8, 1}, 201, 16)]
        public void PlaySimulate(IEnumerable<int> plays, int assert, int totalPlays)
        {
            // Arrange
            var bowling = new Game.Entities.Game();
            //turn 1
            SequencialPlaysMake(bowling, new[] { 1, 4 });
            Assert.Equal(5, bowling.GetScore(Alley, Player));
            //turn 2
            SequencialPlaysMake(bowling, new[] { 4, 5 });
            Assert.Equal(14, bowling.GetScore(Alley, Player));
            //turn 3
            SequencialPlaysMake(bowling, new[] { 6, 4 });
            Assert.Equal(24, bowling.GetScore(Alley, Player));
            //turn 4
            SequencialPlaysMake(bowling, new[] { 5, 5 });
            Assert.Equal(39, bowling.GetScore(Alley, Player));
            //turn 5
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(59, bowling.GetScore(Alley, Player));
            //turn 6
            SequencialPlaysMake(bowling, new[] { 0, 1 });
            Assert.Equal(61, bowling.GetScore(Alley, Player));
            //turn 7
            SequencialPlaysMake(bowling, new[] { 7, 3 });
            Assert.Equal(71, bowling.GetScore(Alley, Player));
            //turn 8
            SequencialPlaysMake(bowling, new[] { 6, 4 });
            Assert.Equal(87, bowling.GetScore(Alley, Player));
            //turn 9
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(107, bowling.GetScore(Alley, Player));
            //turn 10
            SequencialPlaysMake(bowling, new[] { 2, 8 });
            Assert.Equal(127, bowling.GetScore(Alley, Player));
            ////turn BONUS
            bowling.AddPlay(new Play(Player, 6, Alley, DateTime.Now));
            Assert.Equal(133, bowling.GetScore(Alley, Player));
            Assert.Equal(19, bowling.Plays.Count);
        }

        [Fact]
        public void PlaySimulate2()
        {
            var bowling = new Game.Entities.Game();
            //turn 1
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(10, bowling.GetScore(Alley, Player));
            //turn 2
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(30, bowling.GetScore(Alley, Player));
            //turn 3
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(60, bowling.GetScore(Alley, Player));
            //turn 4
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(90, bowling.GetScore(Alley, Player));
            //turn 5
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(120, bowling.GetScore(Alley, Player));
            //turn 6
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(150, bowling.GetScore(Alley, Player));
            //turn 7
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(180, bowling.GetScore(Alley, Player));
            //turn 8
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(210, bowling.GetScore(Alley, Player));
            //turn 9
            SequencialPlaysMake(bowling, new[] { 10 });
            Assert.Equal(240, bowling.GetScore(Alley, Player));
            //turn 10
            SequencialPlaysMake(bowling, new[] { 10, 10 });
            Assert.Equal(290, bowling.GetScore(Alley, Player));
            //turn BONUS
            bowling.AddPlay(new Play(Player, 10, Alley, DateTime.Now));
            Assert.Equal(300, bowling.GetScore(Alley, Player));
        }

        [Theory]
        [InlineData(new[] { 10, 10, 8, 2, 8, 2, 9, 1, 10, 8, 1, 9, 0, 8, 1, 10, 9, 1 }, 171)]
        [InlineData(new[] { 10, 9, 1, 8, 2, 10, 7, 1, 10, 8, 2, 10, 10, 10, 8, 1 }, 201)]
        [InlineData(new[] { 9, 1, 8, 2, 9, 1, 6, 3, 8, 2, 10, 10, 10, 10, 10, 9, 1 }, 221)]
        [InlineData(new[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 1, 3 }, 115)]
        [InlineData(new[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 1, 3, 6 }, 115)]
        [InlineData(new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0)]
        [InlineData(new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, 300)]
        public void PerfectPlaySimulateAndPlayLimitReachedExceptionTest(int[] plays, int score)
        {
            var b = new Entities.Game();
            try
            {
                SequencialPlaysMake(b, plays);
            }
            catch (Exception ex)
            {
                Assert.IsType<PlayLimitReachedException>(ex);
                Assert.NotEqual("", ex.Message);
            }
            finally
            {
                Assert.Equal(score, b.GetScore(Alley, Player));
            }
        }

        [Fact]
        public void PainelScoreTest()
        {
            // Arrange
            var b = new Entities.Game();
            var beginGame = DateTime.Now.AddMinutes(-10);
            b.AddPlay(new Play(Player, 10, Alley, beginGame));
            System.Threading.Thread.Sleep(500);
            
            // Act
            SequencialPlaysMake(b, new[] {10, 10, 10, 10, 10, 10, 10, 10, 10, 10});
            System.Threading.Thread.Sleep(500);
            var endGame = DateTime.Now.AddMinutes(10);
            b.AddPlay(new Play(Player, 10, Alley, endGame));
            var score = b.GetPainel(Alley);
            
            // Assert
            Assert.Equal(300, b.GetScore(Alley, Player));
            Assert.Equal(score.Alley, Alley);
            Assert.Equal(score.BeginGame, beginGame);
            Assert.Equal(score.LastGame, endGame);
            Assert.Single(b.Scores);
        }

        [Fact]
        public void ScoreClearTest()
        {
            // Arrange
            var b = new Entities.Game();
            var plays = new[] {10, 9, 1, 8, 2, 10, 7, 1, 10, 8, 2, 10, 10, 10, 8, 1};
            
            // Act
            SequencialPlaysMake(b, plays);
            var assert1 = b.GetScore(Alley, Player);
            b.GetPainel(Alley).Clear();
            var assert2 = b.GetScore(Alley, Player);
            
            //Assert
            Assert.Equal(201, assert1);
            Assert.Equal(0, assert2);
        }
    }
}