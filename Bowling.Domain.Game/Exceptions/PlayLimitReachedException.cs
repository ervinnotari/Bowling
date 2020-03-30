using System;

namespace Bowling.Domain.Game.Exceptions
{
    [Serializable]
    public class PlayLimitReachedException : SystemException
    {
        public PlayLimitReachedException() : base()
        {
        }
    }
}