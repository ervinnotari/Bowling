using Bowling.Domain.Game.Entities;
using System;
using System.Linq;
using Bowling.Service;

namespace BowlingPainelOnBlazor.Data
{

    public class BowlingService : GameService, IService
    {
        public event EventHandler OnNMSStatusChange;
        private readonly NMSBowlingService _nMSBowlingService;

        public Exception GetError() => _nMSBowlingService.Error;
        public ConnectionStatus GetStatus() => _nMSBowlingService.GetConnectionStatus();

        public BowlingService(NMSBowlingService nMSService)
        {
            _nMSBowlingService = nMSService;
            _nMSBowlingService.OnConnectionSucess += AfterNMSConnectionSucess;
            _nMSBowlingService.OnStatusChange += (s, e) => OnNMSStatusChange?.Invoke(s, e);
#if DEBUG
            var plays = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
            plays.ToList().ForEach(p => AddPlay(new Play("Exemple A", p, "Debug", DateTime.Now)));
            plays = new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            plays.ToList().ForEach(p => AddPlay(new Play("Exemple B", p, "Debug", DateTime.Now)));
#endif
        }

        private void AfterNMSConnectionSucess(Apache.NMS.IConnection conn)
        {
            _nMSBowlingService.OnObjectReciver<Play>(AddPlay);
        }
    }

}
