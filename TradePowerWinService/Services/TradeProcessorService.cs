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
            var powerTrades = await _tradeDataService.GetTradeData();
            var powerTradesDTOs = new List<PowerTradeExportDTO>();
            _exportService.Export(powerTradesDTOs);
        }
    }
}
