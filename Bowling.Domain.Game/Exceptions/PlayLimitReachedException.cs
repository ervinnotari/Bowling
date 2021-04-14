using System;

namespace Bowling.Domain.Game.Exceptions
{
    public class PlayLimitReachedException : Exception
    {
        public PlayLimitReachedException() : base() {}
    }
}