using System;

namespace Bowling.Domain.Game.Entities
{
    public class Painel
    {
        public string Alley { get; set; }
        public DateTime LastGame { get; set; }
        public DateTime BeginGame { get; set; }
        public Players Players { get; }

        public Painel(string alley, DateTime beginGame)
        {
            Alley = alley;
            BeginGame = beginGame;
            Players = new Players();
        }

        public void Clear() => Players.Clear();
    }
}