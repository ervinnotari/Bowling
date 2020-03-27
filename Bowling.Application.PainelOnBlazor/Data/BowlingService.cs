using Bowling.Domain.Game.Entities;
using Bowling.Domain.Game.Interfaces;
using System;
using System.Linq;

namespace BowlingPainelOnBlazor.Data
{

    public class BowlingService
    {
        public event Action<IBusService.ConnectionStatus> OnAmqpStatusChange;
        public readonly IBusService BusService;
        public readonly IGameService Game;
        public readonly ToastService ToastService;
        public object Info { get; private set; }

        public Exception GetError() => BusService.GetError();
        public IBusService.ConnectionStatus GetStatus() => BusService.GetConnectionStatus();

        public BowlingService(IBusService busService, IGameService gameService, ToastService toastService)
        {
            ToastService = toastService;
            Game = gameService;
            BusService = busService;
            BusService.OnConnection += (dynamic c) => BusService.OnObjectReciver<Play>(AddPlay);
            BusService.OnStatusChange += AmqpService_OnStatusChange;
            BusService.ConnectionStartAsync();
#if DEBUG
            var plays = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
            plays.ToList().ForEach(p => AddPlay(new Play("Exemple A", p, "Debug", DateTime.Now)));
            plays = new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            plays.ToList().ForEach(p => AddPlay(new Play("Exemple B", p, "Debug", DateTime.Now)));
#endif
        }

        public void AddPlay(Play play)
        {
            try
            {
                Game.AddPlay(play);
            }
            catch (Exception e)
            {
                ToastService.ShowToast($"Invalid Play: {e.Message}", Microsoft.AspNetCore.Components.Web.ToastLevel.Warning);
            }
        }

        private void AmqpService_OnStatusChange(IBusService.ConnectionStatus obj, dynamic info)
        {
            Info = $"mqtt://{info.Host}:{info.Port}/topic/{info.Topic}";
            if (obj.Equals(IBusService.ConnectionStatus.Error))
                ToastService.ShowToast($"Erro: {GetError().Message}", Microsoft.AspNetCore.Components.Web.ToastLevel.Error);
            OnAmqpStatusChange?.Invoke(obj);
        }
    }

}
