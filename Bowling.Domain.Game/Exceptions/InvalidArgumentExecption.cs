using System;

namespace Bowling.Domain.Game.Exceptions
{
    [Serializable]
    public class InvalidArgumentExecption : SystemException
    {
        public InvalidArgumentExecption() : base() { }
        public InvalidArgumentExecption(string msg) : base(msg) { }
    }
}