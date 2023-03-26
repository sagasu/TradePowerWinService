using Services;
using TradePowerWinService.Models;

namespace TradePowerWinService.Services
{
    public interface ITradeDataService
    {
        Task<IEnumerable<PowerTradeDto>> GetPowerTrades();
    }

    public class TradeDataService : ITradeDataService
    {
        private readonly ILogger<TradeDataService> _logger;
        private readonly IPowerService _powerService;
        private readonly IDateTimeService _dateTimeService;
        private const string FETCHING_TRADES_MESSAGE = "Fetching trades starting.";
        private const string FETCHING_TRADES_SUCCEDED_MESSAGE = "Fetching trades succeded.";
        public TradeDataService(ILogger<TradeDataService> logger, IPowerService powerService, IDateTimeService dateTimeService)
        {
            _logger = logger;
            _powerService = powerService;
            _dateTimeService = dateTimeService;
        }

        public async Task<IEnumerable<PowerTradeDto>> GetPowerTrades()
        {
            _logger.LogInformation(FETCHING_TRADES_MESSAGE);
            var date = _dateTimeService.GetDateTime();
            var trades = await _powerService.GetTradesAsync(date);
            var tradesDtos = trades.Select(x => new PowerTradeDto
            {
                Periods = x.Periods.Select(period => new PowerPeriodDto { Period = period.Period, Volume = period.Volume }),
                Date = x.Date
            });
            _logger.LogInformation(FETCHING_TRADES_SUCCEDED_MESSAGE);
            return tradesDtos;
        }
    }
}
