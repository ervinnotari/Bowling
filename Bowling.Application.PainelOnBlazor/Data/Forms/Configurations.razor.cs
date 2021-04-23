using BowlingPainelOnBlazor.Data;
using Bowling.Domain.Game.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Components.Web
{
    public class ConfigurationsBase : ComponentBase
    {
        [Inject]
        protected IBusService BusService { get; set; }
        [Inject]
        protected IConfiguration Configuration { get; set; }

        protected ConfigurationsModel Model;

        protected override void OnInitialized()
        {
            Model = new ConfigurationsModel(Configuration, BusService);
            Model.OnFormStateChange += () => StateHasChanged();
        }

    }
}