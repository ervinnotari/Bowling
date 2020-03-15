using Bowling.Domain.Game.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bowling.Domain.Game.Interfaces
{
    public interface IGameService
    {
        public event Func<Play, Play> OnPlay;
        public event Action<object> OnChange;

        public void Clear(string alley);
        public void AddPlay(Play play);
        public Painel GetPainel(string alley);
        public Task<Painel> GetScoreAsync(string alley);
        public bool IsExistsAlley(string alley);
        public List<string> GetAlleysName();
    }
}
