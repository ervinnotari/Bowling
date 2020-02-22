using Bowling.Domain.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Domain.Game.Interfaces
{
    public delegate Play PlayEventHandler(Play play);
    public interface IGameService
    {
        public event PlayEventHandler OnPlay;
        public event EventHandler OnChange;

        public void Clear(string alley);
        public void AddPlay(Play play);
        public Painel GetPainel(string alley);
        public Task<Painel> GetScoreAsync(string alley);
        public bool IsExistsAlley(string alley);
        public List<string> GetAlleysName();
    }
}
