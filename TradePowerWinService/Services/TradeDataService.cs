using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public TradeDataService(IPowerService powerService)
        {
            _powerService = powerService;
        }

        public async Task<IEnumerable<PowerTrade>> GetTradeData()
        {
            var date = DateTime.Now.AddHours(-100);
            var trades = await _powerService.GetTradesAsync(date);
            return trades;
        }
    }
}
