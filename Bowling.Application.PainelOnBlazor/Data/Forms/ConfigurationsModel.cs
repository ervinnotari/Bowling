using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Components.Web
{
    public class ConfigurationsModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Identifier too long (100 character limit).")]
        public string Host { get; set; }

        [Range(1, 65000, ErrorMessage = "Accommodation invalid (1-65000).")]
        public int Port { get; set; }

        public string Topic { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Configurations does not tested.")]
        public bool Tested { get; set; } = false;

        public ConfigurationsModel(IConfiguration configuration)
        {
            Host = configuration["Host"];
            Port = int.Parse(configuration["Port"]);
            Topic = configuration["Topic"];
        }

    }
}