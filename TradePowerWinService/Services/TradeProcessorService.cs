using TradePowerWinService.Models;

namespace TradePowerWinService.Services
{
    public interface ITradeProcessorService
    {
        Task ProcessTrade();
        IList<AggregatedPowerDto> GetAggregatedPowerDtos(IEnumerable<PowerTradeDto> powerTradesDtos);
    }

    public class TradeProcessorService : ITradeProcessorService
    {
        public const string START_TIME = "23:00";
        private readonly ITradeDataService _tradeDataService;
        private readonly IExportService _exportService;
        private readonly IDateTimeService _dateTimeService;

        public TradeProcessorService(ITradeDataService tradeDataService, IExportService exportService, IDateTimeService dateTimeService)
        {
            _tradeDataService = tradeDataService;
            _exportService = exportService;
            _dateTimeService = dateTimeService;
        }

        public async Task ProcessTrade()
        {
            var powerTradesDtos = await _tradeDataService.GetTradeData();

            var aggregatedDtos = GetAggregatedPowerDtos(powerTradesDtos);

            _exportService.Export(aggregatedDtos);
        }

        public IList<AggregatedPowerDto> GetAggregatedPowerDtos(IEnumerable<PowerTradeDto> powerTradesDtos)
        {
            var startTime = _dateTimeService.Parse(START_TIME);
            var aggregatedDtos = new List<AggregatedPowerDto>();

            foreach (var powerTradesDto in powerTradesDtos)
            {
                var i = 0;
                foreach (var powerPeriodDto in powerTradesDto.Periods)
                {
                    if (aggregatedDtos.ElementAtOrDefault(i) == null)
                        aggregatedDtos.Add(new AggregatedPowerDto{ LocalTime = startTime.AddHours(i) });
                    
                    aggregatedDtos[i].Volume += powerPeriodDto.Volume;
                    i++;
                }
            }

            return aggregatedDtos;
        }
    }
}
