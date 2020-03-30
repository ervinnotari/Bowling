using System;

namespace Bowling.Domain.Game.Entities
{
    public class Play
    {
        private int _pins;
        public int Pins
        {
            get { return _pins; }
            set
            {
                if (value >= 0 && value <= 10) _pins = value;
                else throw new ArgumentOutOfRangeException(nameof(Pins), value, "Only values ​​from 0 to 10 will be accepted");
            }
        }
        public string Name { get; set; }
        public string Alley { get; set; }
        public DateTime Date { get; set; }

        public Play(string name, int pins, string alley, DateTime date)
        {
            Name = name;
            Pins = pins;
            Alley = alley;
            Date = date;
        }

        public override string ToString()
        {
            return $"Play {{name={Name}, pins={Pins}, alley={Alley}, date={Date}}}";
        }
    }
}
