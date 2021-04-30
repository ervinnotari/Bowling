using BowlingPainelOnBlazor.Data;
using Bowling.Domain.Game.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.AspNetCore.Components.Web
{
    public class ConfigurationsBase : ComponentBase
    {
        [Inject]
        protected IBusService BusService { get; set; }
        [Inject]
        protected IConfiguration Configuration { get; set; }

        protected ConfigurationsModel Model;
        protected EditContext Context;

        protected bool SubmitIsDisable => !(Model.Tested && Context.Validate());
        protected bool TestIsDisable { get; set; } = false;

        protected override void OnInitialized()
        {
            Model = new ConfigurationsModel(Configuration);
            Context = new EditContext(Model);
            Context.OnFieldChanged += Context_OnFieldChanged;
        }

        public void OnSubmit()
        {
            //await BusService.ConnectionStopAsync();
            ////Configuration["Host"] = Host;
            ////Configuration["Port"] = $"{Port}";
            //await BusService.ConnectionStartAsync();
            TestIsDisable = false;
            StateHasChanged();
        }

        public async void TestOnclick()
        {
            if (TestIsDisable) return;

            TestIsDisable = true;
            await BusService.ConnectionStopAsync();
            BusService.OnStatusChange += BusService_OnStatusChange;
            await BusService.ConnectionStartAsync(Model.Host, Model.Port);
            StateHasChanged();
        }

        private void BusService_OnStatusChange(IBusService.ConnectionStatus arg1, object arg2)
        {
            Model.Tested = arg1 == IBusService.ConnectionStatus.Connected;
            BusService.OnStatusChange -= BusService_OnStatusChange;
        }

        private void Context_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            Model.Tested = false;
            TestIsDisable = false;
        }

    }
}