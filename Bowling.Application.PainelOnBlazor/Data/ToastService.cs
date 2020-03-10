using Microsoft.AspNetCore.Components.Web;
using System;
using System.Timers;

namespace BowlingPainelOnBlazor.Data
{

    public class ToastService : IDisposable, IService
    {
        public event Action<string, ToastLevel> OnShow;
        public event Action OnHide;
        private Timer Countdown;
        public int Time { get; set; } = 5000;

        public void ShowToast(string message, ToastLevel level)
        {
            OnShow?.Invoke(message, level);
            StartCountdown();
        }

        private void StartCountdown()
        {
            SetCountdown();

            if (Countdown.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else Countdown.Start();
        }

        private void SetCountdown()
        {
            if (Countdown == null)
            {
                Countdown = new Timer(Time);
                Countdown.Elapsed += (s, a) => OnHide?.Invoke();
                Countdown.AutoReset = false;
            }
        }

        public void Dispose() => Countdown?.Dispose();
    }
}
