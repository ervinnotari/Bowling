using Bowling.Domain.Game.Entities;
using System;
using System.Linq;
using Bowling.Service;
using Bowling.Domain.Game.Interfaces;
using Bowling.Service.NMS;

namespace BowlingPainelOnBlazor.Data
{

    public class BowlingService : GameService, IService
    {
        public event Action<IAmqpService.ConnectionStatus> OnAmqpStatusChange;
        public readonly IAmqpService AmqpService;
        public readonly ToastService ToastService;

        public Exception GetError() => AmqpService.GetError();
        public IAmqpService.ConnectionStatus GetStatus() => AmqpService.GetConnectionStatus();

        public BowlingService(AmqpBowlingService amqpService, ToastService toastService)
        {
            ToastService = toastService;
            AmqpService = amqpService;
            AmqpService.OnConnection += (c) => AmqpService.OnObjectReciver<Play>(AddPlay);
            AmqpService.OnStatusChange += AmqpService_OnStatusChange;
            AmqpService.ConnectionStartAsync();
#if DEBUG
            var plays = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
            plays.ToList().ForEach(p => AddPlay(new Play("Exemple A", p, "Debug", DateTime.Now)));
            plays = new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            plays.ToList().ForEach(p => AddPlay(new Play("Exemple B", p, "Debug", DateTime.Now)));
#endif
        }

        public override void AddPlay(Play play)
        {
            try
            {
                base.AddPlay(play);
            }
            catch (Exception e)
            {
                ToastService.ShowToast($"Invalid Play: {e.Message}", Microsoft.AspNetCore.Components.Web.ToastLevel.Warning);
            }
        }

        private void AmqpService_OnStatusChange(IAmqpService.ConnectionStatus obj)
        {
            if (obj.Equals(IAmqpService.ConnectionStatus.ERROR))
                ToastService.ShowToast($"Erro: {GetError().Message}", Microsoft.AspNetCore.Components.Web.ToastLevel.Error);
            OnAmqpStatusChange?.Invoke(obj);
        }
    }

}
