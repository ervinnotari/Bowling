using Bowling.Domain.Game.Entities;
using BowlingPainelOnBlazor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.Web
{
    public class GameBase : ComponentBase, IDisposable
    {
        [Parameter] public string Alley { get; set; } = "01";
        [Inject] protected BowlingService BowlingService { get; set; }
        protected Painel Painel { get; set; }

        protected override Task OnParametersSetAsync()
        {
            BowlingService.OnChange += BowlingService_OnChange;
            BowlingService_OnChange(null, EventArgs.Empty);
            return base.OnParametersSetAsync();
        }

        private void BowlingService_OnChange(object sender, EventArgs e) => InvokeAsync(RefrashOnChange);

        private async void RefrashOnChange()
        {
            Painel = await BowlingService.GetScoreAsync(Alley);
            StateHasChanged();
        }

        public void Dispose()
        {
            BowlingService.OnChange -= BowlingService_OnChange;
        }
    }
}
