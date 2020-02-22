using Bowling.Domain.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Service
{
    class GameCleanEventArgs : EventArgs
    {
        public bool Cancel = false;
    }
}
