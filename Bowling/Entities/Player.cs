namespace BowlingGame.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public Frames Frames { get; set; } = new Frames();
    }
}
