using System;

namespace Bowling.Service
{
    class GameCleanEventArgs : EventArgs
    {
        public bool Cancel = false;
    }
}
