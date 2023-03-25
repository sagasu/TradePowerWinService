using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradePowerWinService.Services
{
    public interface IDateTimeService
    {
        DateTime GetDateTime ();
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime GetDateTime () => DateTime.Now;
    }
}
