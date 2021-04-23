using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Components.Forms;
using BowlingPainelOnBlazor.Data;
using Microsoft.Extensions.Configuration;
using Bowling.Domain.Game.Interfaces;

namespace Microsoft.AspNetCore.Components.Web
{
    public class ConfigurationsModel
    {
        public event Action OnFormStateChange;

        [Required]
        [StringLength(100, ErrorMessage = "Identifier too long (100 character limit).")]
        public string Host { get; set; }

        [Range(1, 65000, ErrorMessage = "Accommodation invalid (1-65000).")]
        public int Port { get; set; }

        public string Topic { get; set; }

        public bool SubmitIsDisable => _loading || !_tested || !Context.Validate();

        public EditContext Context;

        public bool Tested
        {
            get { return _tested; }
            set
            {
                if (value == _tested) return;
                _tested = value;
                OnFormStateChange?.Invoke();
            }
        }

        private bool _tested = false;
        private bool _loading = false;
        private readonly IBusService _busService;
        private readonly IConfiguration _configuration;

        public ConfigurationsModel(IConfiguration configuration, IBusService busService)
        {
            Host = configuration["Host"];
            Port = int.Parse(configuration["Port"]);
            Topic = configuration["Topic"];
            _busService = busService;
            _configuration = configuration;
            Context = new EditContext(this);
            Context.OnFieldChanged += (s, e) => Tested = false;
        }

        public async void OnSubmit()
        {
            _loading = true;
            _configuration["Host"] = Host;
            _configuration["Port"] = $"{Port}";
            await _busService.ConnectionStopAsync();
            await _busService.ConnectionStartAsync();
            _loading = false;
        }

        public void TestOnclick()
        {
            Tested = Context.Validate();
        }
    }
}