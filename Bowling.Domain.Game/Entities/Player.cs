namespace Bowling.Domain.Game.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public Frames Frames { get; } = new Frames();
    }
}
