using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Game.Entities
{
    public class Game
    {
        public List<Play> Plays { get; set; } = new List<Play>();
        public Dictionary<string, Painel> Scores { get; private set; } = new Dictionary<string, Painel>();
        public Painel GetPainel(string alley) => Scores.FirstOrDefault(s => s.Key == alley).Value;
        public int GetScore(string alley, string player) => GetPainel(alley).Players.FirstOrDefault(s => s.Name == player).Frames.GetTotal();
        public void AddPlay(Play play)
        {
            Plays.Add(play);
            if (!Scores.ContainsKey(play.Alley))
            {
                Scores.Add(play.Alley, new Painel(play.Alley, play.Date));
            }
            var score = Scores[play.Alley];
            if (!score.Players.Any(p => p.Name == play.Name))
            {
                score.Players.Add(new Player() { Name = play.Name });
            }
            score.LastGame = play.Date;
            score.Players.AddPlay(play);
        }
    }
}
