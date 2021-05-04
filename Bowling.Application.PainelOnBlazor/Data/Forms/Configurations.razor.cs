﻿using BowlingPainelOnBlazor.Data;
using Bowling.Domain.Game.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using Bowling.Infra.Utilities;
using System;
using System.Threading.Tasks;

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
        private TestBusConfigurations BusConfigurations;

        protected override void OnInitialized()
        {
            BusConfigurations = new TestBusConfigurations(Configuration);
            Model = new ConfigurationsModel()
            {
                Host = BusConfigurations.BrokerUri.Host,
                Port = BusConfigurations.BrokerUri.Port,
                Topic = BusConfigurations.Topic,
            };
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

        public async Task TestOnclick()
        {
            if (TestIsDisable) return;
            TestIsDisable = true;
            await BusService.ConnectionStopAsync();
            BusService.OnStatusChange += BusService_OnStatusChange;
            var builder = new UriBuilder(BusConfigurations.BrokerUri);
            await BusService.ConnectionStartAsync(builder.Uri);
            StateHasChanged();
        }

        private void BusService_OnStatusChange(IBusService.ConnectionStatus arg1, IBusService.ConnectionInfo arg2)
        {
            Model.Tested = arg1 == IBusService.ConnectionStatus.Connected;
            BusService.OnStatusChange -= BusService_OnStatusChange;
        }

        private void Context_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            Model.Tested = false;
            TestIsDisable = false;
        }

        private class TestBusConfigurations : AbstractBusConfigurations
        {
            public TestBusConfigurations(IConfiguration configuration) : base(configuration) { }
        }

    }
}