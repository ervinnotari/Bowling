using Bowling.Domain.Game.Entities;
using Bowling.Domain.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bowling.Service
{
    public class GameService : IGameService
    {
        public event PlayEventHandler OnPlay;
        public event EventHandler OnChange;
        protected readonly Domain.Game.Entities.Game Game = new Domain.Game.Entities.Game();

        public void AddPlay(Play play)
        {
            Game.AddPlay(OnPlay?.Invoke(play) ?? play);
            OnChange?.Invoke(play, EventArgs.Empty);
        }

        public void Clear(string alley)
        {
            var args = new GameCleanEventArgs();
            OnChange?.Invoke(alley, args);
            if (!args.Cancel) GetPainel(alley).Clear();
        }

        public Painel GetPainel(string alley)
        {
            return Game.GetPainel(alley);
        }

        public Task<Painel> GetScoreAsync(string alley)
        {
            return Task.FromResult(GetPainel(alley));
        }

        public bool IsExistsAlley(string alley)
        {
            return Game.Scores.ContainsKey(alley);
        }

        public List<string> GetAlleysName()
        {
            return Game.Scores.Keys.ToList();
        }
    }
}
