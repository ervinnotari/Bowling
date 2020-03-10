using Bowling.Domain.Game.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowling.Application.PlayWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly NMSBowlingService _nMSBowlingService;

        private readonly int _delay = int.Parse(Environment.GetEnvironmentVariable("BOWLING_DELAY") ?? "1000");
        private readonly string _playeName = Environment.GetEnvironmentVariable("BOWLING_PLAYENAME") ?? "Teste";
        private readonly string _alley = Environment.GetEnvironmentVariable("BOWLING_ALLEY") ?? "01";

        public Worker(ILogger<Worker> logger, IOptionsMonitor<NMSConfigurations> options, NMSBowlingService nMSBowlingService)
        {
            _logger = logger;
            _nMSBowlingService = nMSBowlingService;
            _nMSBowlingService.OnStatusChange += _nMSBowlingService_OnStatusChange;
        }

        private void _nMSBowlingService_OnStatusChange()
        {
            _logger.LogInformation($"NMS Status: {_nMSBowlingService.GetConnectionStatus().ToString()}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_nMSBowlingService.GetConnectionStatus() == ConnectionStatus.CONNECTED)
                {
                    var n = new Random(DateTime.Now.Millisecond);
                    var play = new Play(_playeName, n.Next(0, 11), _alley, DateTime.Now);
                    _logger.LogInformation($"{play}");
                    _nMSBowlingService.SendObject(play);
                }
                await Task.Delay(_delay, stoppingToken);
            }
        }
    }
}
