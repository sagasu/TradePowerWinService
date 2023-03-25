using TradePowerWinService.Models;

namespace TradePowerWinService.Services
{
    public interface ITradeProcessorService
    {
        Task ProcessTrade();
    }

    public class TradeProcessorService : ITradeProcessorService
    {
        private readonly ITradeDataService _tradeDataService;
        private readonly IExportService _exportService;

        public TradeProcessorService(ITradeDataService tradeDataService, IExportService exportService)
        {
            _tradeDataService = tradeDataService;
            _exportService = exportService;
        }

        public async Task ProcessTrade()
        {
            var powerTradesDtos = await _tradeDataService.GetTradeData();
            var aggregatedDtos = powerTradesDtos.Select(powerTrade => new AggregatedPowerDto
            {
                LocalTime = powerTrade.Dates,
                Volume = powerTrade.Periods.Sum(_ => _.Volume)
            });
            _exportService.Export(aggregatedDtos);
        }
    }
}
