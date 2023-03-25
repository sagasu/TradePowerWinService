namespace TradePowerWinService.Models
{
    public class PowerTradeDto
    {
        public DateTime Date { get; set; }
        public IEnumerable<PowerPeriodDto> Periods { get; set; } = new List<PowerPeriodDto>();
    }
}
