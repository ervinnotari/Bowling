using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bowling.Domain.Game.Entities;
using Bowling.Domain.Game.Interfaces;

namespace Bowling.Service.Game
{
    public sealed class GameService : IGameService
    {
        public event Func<Play, Play> OnPlay;
        public event Action<object> OnChange;
        private readonly Domain.Game.Entities.Game _game;

        public GameService()
        {
            _game = new Domain.Game.Entities.Game();
        }

        public void AddPlay(Play play)
        {
            _game.AddPlay(OnPlay?.Invoke(play) ?? play);
            OnChange?.Invoke(play);
        }

        public void Clear(string alley)
        {
            GetPainel(alley).Clear();
            OnChange?.Invoke(alley);
        }

        public Painel GetPainel(string alley)
        {
            return _game.GetPainel(alley);
        }

        public Task<Painel> GetScoreAsync(string alley)
        {
            return Task.FromResult(GetPainel(alley));
        }

        public bool IsExistsAlley(string alley)
        {
            return _game.Scores.ContainsKey(alley);
        }

        public List<string> GetAlleysName()
        {
            return _game.Scores.Keys.ToList();
        }
    }
}
