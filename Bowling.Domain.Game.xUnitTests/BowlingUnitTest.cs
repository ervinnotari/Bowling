using System;
using System.Collections.Generic;
using Bowling.Domain.Game.Entities;
using Xunit;
using System.Linq;

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
            
            // Act
            SequencialPlaysMake(bowling, plays);
            
            // Assert
            Assert.Equal(assert, bowling.GetScore(Alley, Player));
            Assert.Equal(totalPlays, bowling.Plays.Count);
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