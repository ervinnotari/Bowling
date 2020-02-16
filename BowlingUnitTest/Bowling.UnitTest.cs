using BowlingGame.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingUnitTest
{
    [TestClass]
    public class BowlingUnitTest
    {
        private const string PLAYER = "José";
        private const string ALLEY = "2";

        public static void SequencialPlaysMake(Bowling b, int[] plays)
        {
            plays.ToList().ForEach(p =>
            {
                b.AddPlay(new Play(PLAYER, p, ALLEY, DateTime.Now));
            });
        }

        [TestMethod]
        public void PlaySimulate1()
        {
            var bowling = new Bowling();
            //turn 1
            SequencialPlaysMake(bowling, new int[] { 1, 4 });
            Assert.AreEqual(5, bowling.GetScore(ALLEY, PLAYER), "1º Turno");
            //turn 2
            SequencialPlaysMake(bowling, new int[] { 4, 5 });
            Assert.AreEqual(14, bowling.GetScore(ALLEY, PLAYER), "2º Turno");
            //turn 3
            SequencialPlaysMake(bowling, new int[] { 6, 4 });
            Assert.AreEqual(24, bowling.GetScore(ALLEY, PLAYER), "3º Turno");
            //turn 4
            SequencialPlaysMake(bowling, new int[] { 5, 5 });
            Assert.AreEqual(39, bowling.GetScore(ALLEY, PLAYER), "4º Turno");
            //turn 5
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(59, bowling.GetScore(ALLEY, PLAYER), "5º Turno");
            //turn 6
            SequencialPlaysMake(bowling, new int[] { 0, 1 });
            Assert.AreEqual(61, bowling.GetScore(ALLEY, PLAYER), "6º Turno");
            //turn 7
            SequencialPlaysMake(bowling, new int[] { 7, 3 });
            Assert.AreEqual(71, bowling.GetScore(ALLEY, PLAYER), "7º Turno");
            //turn 8
            SequencialPlaysMake(bowling, new int[] { 6, 4 });
            Assert.AreEqual(87, bowling.GetScore(ALLEY, PLAYER), "8º Turno");
            //turn 9
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(107, bowling.GetScore(ALLEY, PLAYER), "9º Turno");
            //turn 10
            SequencialPlaysMake(bowling, new int[] { 2, 8 });
            Assert.AreEqual(127, bowling.GetScore(ALLEY, PLAYER), "10º Turno");
            ////turn BONUS
            bowling.AddPlay(new Play(PLAYER, 6, ALLEY, DateTime.Now));
            Assert.AreEqual(133, bowling.GetScore(ALLEY, PLAYER), "BONUS Turno");
            Assert.AreEqual(19, bowling.Plays.Count);
        }

        [TestMethod]
        public void PlaySimulate2()
        {
            var bowling = new Bowling();
            //turn 1
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(10, bowling.GetScore(ALLEY, PLAYER), "1º Turno");
            //turn 2
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(30, bowling.GetScore(ALLEY, PLAYER), "2º Turno");
            //turn 3
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(60, bowling.GetScore(ALLEY, PLAYER), "3º Turno");
            //turn 4
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(90, bowling.GetScore(ALLEY, PLAYER), "4º Turno");
            //turn 5
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(120, bowling.GetScore(ALLEY, PLAYER), "5º Turno");
            //turn 6
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(150, bowling.GetScore(ALLEY, PLAYER), "6º Turno");
            //turn 7
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(180, bowling.GetScore(ALLEY, PLAYER), "7º Turno");
            //turn 8
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(210, bowling.GetScore(ALLEY, PLAYER), "8º Turno");
            //turn 9
            SequencialPlaysMake(bowling, new int[] { 10 });
            Assert.AreEqual(240, bowling.GetScore(ALLEY, PLAYER), "9º Turno");
            //turn 10
            SequencialPlaysMake(bowling, new int[] { 10, 10 });
            Assert.AreEqual(290, bowling.GetScore(ALLEY, PLAYER), "10º Turno");
            //turn BONUS
            bowling.AddPlay(new Play(PLAYER, 10, ALLEY, DateTime.Now));
            Assert.AreEqual(300, bowling.GetScore(ALLEY, PLAYER), "BONUS Turno");
        }

        [TestMethod]
        public void PlaySimulate3()
        {
            var b = new Bowling();
            var plays = new int[] { 10, 10, 8, 2, 8, 2, 9, 1, 10, 8, 1, 9, 0, 8, 1, 10, 9, 1 };
            SequencialPlaysMake(b, plays);
            Assert.AreEqual(171, b.GetScore(ALLEY, PLAYER), "BONUS Turno");
        }

        [TestMethod]
        public void PlaySimulate4()
        {
            var b = new Bowling();
            var plays = new int[] { 9, 1, 8, 2, 9, 1, 6, 3, 8, 2, 10, 10, 10, 10, 10, 9, 1 };
            SequencialPlaysMake(b, plays);
            Assert.AreEqual(221, b.GetScore(ALLEY, PLAYER), "BONUS Turno");
        }

        [TestMethod]
        public void PlaySimulate5()
        {
            var b = new Bowling();
            var plays = new int[] { 10, 9, 1, 8, 2, 10, 7, 1, 10, 8, 2, 10, 10, 10, 8, 1 };
            SequencialPlaysMake(b, plays);
            Assert.AreEqual(201, b.GetScore(ALLEY, PLAYER), "BONUS Turno");
        }

        [TestMethod]
        public void PainelScoreTest()
        {
            var b = new Bowling();
            var beginGame = DateTime.Now.AddMinutes(-10);
            b.AddPlay(new Play(PLAYER, 10, ALLEY, beginGame));
            System.Threading.Thread.Sleep(500);
            SequencialPlaysMake(b, new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            System.Threading.Thread.Sleep(500);
            var endGame = DateTime.Now.AddMinutes(10);
            b.AddPlay(new Play(PLAYER, 10, ALLEY, endGame));
            Assert.AreEqual(300, b.GetScore(ALLEY, PLAYER));

            var score = b.GetPainel(ALLEY);
            Assert.AreEqual(score.Alley, ALLEY);
            Assert.AreEqual(score.BeginGame, beginGame);
            Assert.AreEqual(score.LastGame, endGame);
            Assert.AreEqual(b.Scores.Count, 1);
        }
    }
}
