using BowlingGame;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingPainelOnBlazor.Data
{
    public delegate void PlayEventHandler(Play play);
    public delegate void VoidEventHandler();
    public class BowlingService
    {
        public Bowling Bowling { get; } = new Bowling();

        public event PlayEventHandler BeforePlay;
        public event PlayEventHandler AfterPlay;
        public event VoidEventHandler BeforeChange;
        public event VoidEventHandler AfterChange;

        /*       public BowlingService()
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
               }*/

        internal void Clear(string alley)
        {
            BeforeChange?.Invoke();
            Bowling.GetPainel(alley).Clear();
            AfterChange?.Invoke();
        }
        internal Painel GetPainel(string alley)
        {
            return Bowling.GetPainel(alley);
        }
        internal void AddPlay(Play play)
        {
            BeforePlay?.Invoke(play);
            BeforeChange?.Invoke();
            Bowling.AddPlay(play);
            AfterChange?.Invoke();
            AfterPlay?.Invoke(play);
        }
        internal Task<Painel> GetScoreAsync(string alley)
        {
            var painel = Bowling.GetPainel(alley);
            return Task.FromResult(painel);
        }
    }
}
