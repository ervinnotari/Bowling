using Bowling.Domain.Game.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Game.Entities
{
    public class Frames : List<Frame>
    {
        private enum Bonus { STRIKE1, STRIKE2, SPARE }
        private readonly Stack<KeyValuePair<Bonus, Frame>> _bonus = new Stack<KeyValuePair<Bonus, Frame>>();
        private int Turn => this.Count(f => !f.IsOpen());
        private bool IsEndGame() => Turn == 10 && ((this.LastOrDefault()?.Balls.ElementAtOrDefault(0) == 10 && this.LastOrDefault()?.Balls.Count == 3) || ((this.LastOrDefault()?.Balls.ElementAtOrDefault(0) + this.LastOrDefault()?.Balls.ElementAtOrDefault(1)) < 10 && this.LastOrDefault()?.Balls.Count == 2));
        public int GetTotal() => this.LastOrDefault()?.Score ?? 0;
        public void AddPlay(Play play)
        {
            if (IsEndGame()) throw new PlayLimitReachedException();

            var last = this.ElementAtOrDefault(Turn - 1);
            var strike2 = default(Frame);

            while (_bonus.Count > 0)
            {
                var kp = _bonus.Pop();
                kp.Value.Bonus += play.Pins;
                if (kp.Key == Bonus.STRIKE1)
                    strike2 = kp.Value;
            }

            if (strike2 != null)
                _bonus.Push(new KeyValuePair<Bonus, Frame>(Bonus.STRIKE2, strike2));
            if (!this.Exists(f => f.IsOpen()) && Turn < 10)
                Add(new Frame(last));

            var frame = this.LastOrDefault();
            frame.Balls.Add(play.Pins);

            if (frame.IsStrike() && Turn < 10)
                _bonus.Push(new KeyValuePair<Bonus, Frame>(Bonus.STRIKE1, frame));
            if (frame.IsSpare() && Turn < 10)
                _bonus.Push(new KeyValuePair<Bonus, Frame>(Bonus.SPARE, frame));
        }

    }
}
