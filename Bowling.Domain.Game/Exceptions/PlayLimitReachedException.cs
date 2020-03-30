using System;
using System.Runtime.Serialization;

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