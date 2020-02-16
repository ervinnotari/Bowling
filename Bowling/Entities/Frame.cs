using System.Collections.Generic;
using System.Linq;

namespace BowlingGame.Entities
{
    public class Frame
    {
        public int Score => (Last?.Score ?? 0) + Balls.Sum() + Bonus;
        public List<int> Balls { get; set; } = new List<int>();
        internal Frame Last { get; set; }
        internal int Bonus { get; set; } = 0;
        public int GetAttempt() => Balls.Count;
        public bool IsOpen() => (Balls.Sum() < 10 && Balls.Count < 2);
        public bool IsSpare() => (Balls.Sum() == 10 && Balls.Count == 2);
        public bool IsStrike() => (Balls.Sum() == 10 && Balls.Count == 1);

        public Frame(Frame last) { Last = last; }
        public override string ToString()
        {
            return $"{this.GetType().Name} {{Balls:[{string.Join(",", Balls)}] Score:{Score}}}";
        }
    }
}
