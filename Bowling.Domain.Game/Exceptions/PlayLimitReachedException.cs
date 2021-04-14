using System;
using System.Runtime.Serialization;

namespace Bowling.Domain.Game.Exceptions
{
    [Serializable]
    public class PlayLimitReachedException : SystemException, ISerializable
    {
        public PlayLimitReachedException() : base() {}
    }
}