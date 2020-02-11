using BowlingGame;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingPainelOnBlazor.Data
{
    public delegate void PlayEvent(Play play);
    public class BowlingService
    {

        public Bowling Bowling { get; } = new Bowling();

        public event PlayEvent BeforePlay;
        public event PlayEvent AfterPlay;

        public BowlingService()
        {
            var t = new System.Timers.Timer(1000);
            t.Elapsed += ProgramedPlays;
            t.AutoReset = true;
            t.Start();
        }

        static int[] plays1 = new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        int plays1Idx = 0;
        static int[] plays2 = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
        int plays2Idx = 0;
        static int playCount = 0;
        private void ProgramedPlays(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var playerIdx = playCount % 2;
                var play = default(Play);
                if (playerIdx == 0 && plays1Idx < plays1.Length)
                    play = new Play("Teste", plays1.ElementAtOrDefault(plays1Idx++), "01", DateTime.Now);
                else if (playerIdx == 1 && plays2Idx < plays2.Length)
                    play = new Play("Teste1", plays2.ElementAtOrDefault(plays2Idx++), "02", DateTime.Now);
                if (play != null)
                    AddPlay(play);
                playCount++;
            }
            catch (Exception)
            {
            }
        }

        public void AddPlay(Play play)
        {
            BeforePlay?.Invoke(play);
            Bowling.AddPlay(play);
            AfterPlay?.Invoke(play);
        }
        public Task<Painel> GetScoreAsync(string alley)
        {
            var painel = Bowling.GetPainel(alley);
            return Task.FromResult(painel);
        }
    }
}
