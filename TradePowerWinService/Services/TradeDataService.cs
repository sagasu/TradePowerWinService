using Services;
using TradePowerWinService.Models;

namespace TradePowerWinService.Services
{
    public interface ITradeDataService
    {
        Task<IEnumerable<PowerTradeDto>> GetTradeData();
    }

    public class TradeDataService : ITradeDataService
    {
        private readonly IPowerService _powerService;
        private readonly IDateTimeService _dateTimeService;

        public TradeDataService(IPowerService powerService, IDateTimeService dateTimeService)
        {
            _powerService = powerService;
            _dateTimeService = dateTimeService;
        }

        public async Task<IEnumerable<PowerTradeDto>> GetTradeData()
        {
            var date = _dateTimeService.GetDateTime();
            var trades = await _powerService.GetTradesAsync(date);
            var tradesDtos = trades.Select(x => new PowerTradeDto
            {
                Periods = x.Periods.Select(period => new PowerPeriodDto { Period = period.Period, Volume = period.Volume }),
                Date = x.Date
            });
            return tradesDtos;
        }
    }
}
