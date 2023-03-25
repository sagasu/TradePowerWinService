using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradePowerWinService.Services
{
    public class TimedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedService> _logger;
        private readonly ITradeProcessorService _tradeProcessorService;
        private Timer _timer;

        public TimedService(ILogger<TimedService> logger, ITradeProcessorService tradeProcessorService)
        {
            _logger = logger;
            _tradeProcessorService = tradeProcessorService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Task.CompletedTask;

            _logger.LogInformation("Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            _logger.LogInformation("Service is running.");
            await _tradeProcessorService.ProcessTrade();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
