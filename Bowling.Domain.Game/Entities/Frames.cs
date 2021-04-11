using Bowling.Domain.Game.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Game.Entities
{
    public class Frames : List<Frame>
    {
        private enum Bonus { Strike1, Strike2, Spare }
        private readonly Stack<KeyValuePair<Bonus, Frame>> _bonus = new Stack<KeyValuePair<Bonus, Frame>>();
        private int Turn => this.Count(f => !f.IsOpen());
        private bool IsEndGame()
        {
            if (Turn == 10)
            {
                var balls = this.LastOrDefault().Balls;
                var e0 = balls.ElementAtOrDefault(0);
                var e1 = balls.ElementAtOrDefault(1);
                var _3Strike = (e0 == 10 && balls.Count == 3);
                if (_3Strike) return true;
                var noStrike = (e0 + e1) < 10;
                if (noStrike)
                    if (balls.Count == 2)
                        return true;
                return false;
            }
            return false;
        }
        public int GetTotal() => this.LastOrDefault().Score;
        public void AddPlay(Play play)
        {
            if (IsEndGame()) throw new PlayLimitReachedException();

            var last = this.ElementAtOrDefault(Turn - 1);
            var strike2 = default(Frame);

            while (_bonus.Count > 0)
            {
                var kp = _bonus.Pop();
                kp.Value.Bonus += play.Pins;
                if (kp.Key == Bonus.Strike1)
                    strike2 = kp.Value;
            }

            if (strike2 != null)
                _bonus.Push(new KeyValuePair<Bonus, Frame>(Bonus.Strike2, strike2));
            if (!this.Exists(f => f.IsOpen()) && Turn < 10)
                Add(new Frame(last));

            var frame = this.LastOrDefault();
            if (frame != null)
            {
                frame.Balls.Add(play.Pins);
                if (frame.IsStrike() && Turn < 10)
                    _bonus.Push(new KeyValuePair<Bonus, Frame>(Bonus.Strike1, frame));
                if (frame.IsSpare() && Turn < 10)
                    _bonus.Push(new KeyValuePair<Bonus, Frame>(Bonus.Spare, frame));
            }
        }

    }
}
