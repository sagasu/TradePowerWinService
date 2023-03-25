using Services;
namespace TradePowerWinService.Services
{
    public interface ITradeDataService
    {
        Task<IEnumerable<PowerTrade>> GetTradeData();
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

        public async Task<IEnumerable<PowerTrade>> GetTradeData()
        {
            var date = _dateTimeService.GetDateTime();
            var trades = await _powerService.GetTradesAsync(date);
            return trades;
        }
    }
}
