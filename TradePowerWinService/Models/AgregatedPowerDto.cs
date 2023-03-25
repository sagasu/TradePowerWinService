using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradePowerWinService.Models
{
    public class AggregatedPowerDto
    {
        public DateTime LocalTime { get; set; }
        public double Volume { get; set; }
    }
}
