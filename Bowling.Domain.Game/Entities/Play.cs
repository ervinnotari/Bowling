using System;

namespace Bowling.Domain.Game.Entities
{
    public class Play
    {
        private const string ArgumentOutOfRangeMessage = "Only values ​​from 0 to 10 will be accepted";

        private int _pins;

        public int Pins
        {
            get => _pins;
            set => _pins = PinsCheck(value);
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

        private int PinsCheck(int pins)
        {
            if (pins >= 0 && pins <= 10) return pins;
            else throw new ArgumentOutOfRangeException(nameof(pins), pins, ArgumentOutOfRangeMessage);
        }
    }
}