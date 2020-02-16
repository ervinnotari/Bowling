using System;

namespace BowlingGame.Entities
{
    public class Painel
    {
        public string Alley { get; set; }
        public DateTime LastGame { get; set; }
        public DateTime BeginGame { get; set; }
        public Players Players { get; set; } = new Players();

        public Painel(string alley, DateTime beginGame)
        {
            Alley = alley;
            BeginGame = beginGame;
        }

        public void Clear() => Players.Clear();

    }
}
