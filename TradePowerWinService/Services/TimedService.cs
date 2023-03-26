using Microsoft.Extensions.Options;
using TradePowerWinService.Config;

namespace TradePowerWinService.Services
{
    public class TimedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedService> _logger;
        private readonly ITradeProcessorService _tradeProcessorService;
        private readonly ServiceConfig _serviceConfig;
        private Timer? _timer;
        private const string STARTING_MESSAGE = "Service is starting.";
        private const string RUNNING_MESSAGE = "Service is running.";
        private const string STOPPING_MESSAGE = "Service is stopping.";
        private const string ERROR_MESSAGE = "Error happened, restarting entire service.";

        public TimedService(ILogger<TimedService> logger, ITradeProcessorService tradeProcessorService, IOptions<ServiceConfig> serviceConfig)
        {
            _logger = logger;
            _tradeProcessorService = tradeProcessorService;
            _serviceConfig = serviceConfig.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(STARTING_MESSAGE);

            _timer = new Timer(ProcessTrade, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(_serviceConfig.RunEveryMinutes));

            return Task.CompletedTask;
        }

        public async void ProcessTrade(object? state)
        {
            _logger.LogInformation(RUNNING_MESSAGE);
            try
            {
                await _tradeProcessorService.ProcessTrade();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ERROR_MESSAGE);
                Environment.Exit(1);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(STOPPING_MESSAGE);

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
