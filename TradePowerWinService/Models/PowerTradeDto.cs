namespace TradePowerWinService.Models
{
    public class PowerTradeDto
    {
        public DateTime Dates { get; set; }
        public IEnumerable<PowerPeriodDto> Periods { get; set; } = new List<PowerPeriodDto>();
    }
}
