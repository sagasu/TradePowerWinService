using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _exportService.Export(powerTrades);
        }
    }
}
