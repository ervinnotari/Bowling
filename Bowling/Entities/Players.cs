using System.Collections.Generic;
using System.Linq;

namespace BowlingGame.Entities
{
    public class Players : List<Player>
    {
        public void AddPlay(Play play)
        {
            var player = this.FirstOrDefault(p => p.Name == play.Name);
            if (player == null)
            {
                player = new Player() { Name = play.Name };
                Add(player);
            }
            player.Frames.AddPlay(play);
        }
    }
}
