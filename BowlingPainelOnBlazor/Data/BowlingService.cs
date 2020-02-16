using BowlingGame.Entities;
using System.Threading.Tasks;
using System;

namespace BowlingPainelOnBlazor.Data
{
    public delegate void PlayEventHandler(Play play);
    public delegate void VoidEventHandler();
    public class BowlingService : IBowlingService
    {
        private readonly NMSBowlingService _nMSBowlingService;
        public Bowling Bowling { get; } = new Bowling();

        public event PlayEventHandler BeforePlay;
        public event PlayEventHandler AfterPlay;
        public event VoidEventHandler BeforeChange;
        public event VoidEventHandler AfterChange;
        public event VoidEventHandler OnStatusChange;

        public Exception GetError() => _nMSBowlingService.Error;
        public ConnectionStatus GetStatus() => _nMSBowlingService.GetConnectionStatus();

        public BowlingService(NMSBowlingService nMSService)
        {
            _nMSBowlingService = nMSService;
            _nMSBowlingService.OnConnectionSucess += AfterNMSConnectionSucess;
            _nMSBowlingService.OnStatusChange += NMSBowlingServiceOnStatusChange;
        }

        private void NMSBowlingServiceOnStatusChange() => OnStatusChange?.Invoke();

        private void AfterNMSConnectionSucess(Apache.NMS.IConnection conn)
        {
            _nMSBowlingService.OnObjectReciver<Play>(AddPlay);
        }

        internal void Clear(string alley)
        {
            BeforeChange?.Invoke();
            Bowling.GetPainel(alley).Clear();
            AfterChange?.Invoke();
        }
        internal Painel GetPainel(string alley)
        {
            return Bowling.GetPainel(alley);
        }
        internal void AddPlay(Play play)
        {
            try
            {
                BeforePlay?.Invoke(play);
                BeforeChange?.Invoke();
                Bowling.AddPlay(play);
                AfterChange?.Invoke();
                AfterPlay?.Invoke(play);
            }
            catch (Exception) { }
        }
        internal Task<Painel> GetScoreAsync(string alley)
        {
            var painel = Bowling.GetPainel(alley);
            return Task.FromResult(painel);
        }
    }

}
