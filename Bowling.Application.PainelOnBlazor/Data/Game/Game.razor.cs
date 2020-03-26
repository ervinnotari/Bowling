using Bowling.Domain.Game.Entities;
using BowlingPainelOnBlazor.Data;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components.Web
{
    public class GameBase : ComponentBase
    {
        [Parameter] public string Alley { get; set; } = "01";
        [Inject] protected BowlingService BowlingService { get; set; }
        protected Painel Painel { get; set; }

        protected override Task OnParametersSetAsync()
        {
            BowlingService.Game.OnChange += BowlingService_OnChange;
            BowlingService_OnChange(null);
            return base.OnParametersSetAsync();
        }

        private void BowlingService_OnChange(object sender) => InvokeAsync(RefrashOnChange);

        private async void RefrashOnChange()
        {
            Painel = await BowlingService.Game.GetScoreAsync(Alley);
            StateHasChanged();
        }
         ~GameBase()
        {
            BowlingService.Game.OnChange -= BowlingService_OnChange;
        }
    }
}
