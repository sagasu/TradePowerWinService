using CsvHelper.Configuration.Attributes;

namespace TradePowerWinService.Models
{
    public class AggregatedPowerDto
    {
        [Name("Local Time")]
        [Format("HH:mm")]
        public DateTime LocalTime { get; set; }
        public double Volume { get; set; }
    }
}
