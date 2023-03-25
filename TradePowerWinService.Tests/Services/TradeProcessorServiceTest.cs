using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradePowerWinService.Models;
using TradePowerWinService.Services;

namespace TradePowerWinService.Tests.Services
{
    [TestClass]
    public class TradeProcessorServiceTest
    {
        [TestMethod]
        public void GetAggregatedPowerDtos_MultiplePeriods_VolumeAggregatedCorrectly()
        {
            var tradeDataService = new Mock<ITradeDataService>();
            var exportService = new Mock<IExportService>();
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.Parse(It.IsAny<string>())).Returns(DateTime.Parse("23:00"));
            var powerTradeDtos = new List<PowerTradeDto>
            {
                new PowerTradeDto{Date = DateTime.Now, Periods = new List<PowerPeriodDto>{ new PowerPeriodDto{Period = 0, Volume = 1}, new PowerPeriodDto{Period = 1, Volume = 2}, new PowerPeriodDto{Period = 2, Volume = 3}}},
                new PowerTradeDto{Date = DateTime.Now, Periods = new List<PowerPeriodDto>{ new PowerPeriodDto{Period = 0, Volume = 22}, new PowerPeriodDto{Period = 1, Volume = 22}, new PowerPeriodDto{Period = 2, Volume = 33}}},
                new PowerTradeDto{Date = DateTime.Now, Periods = new List<PowerPeriodDto>{ new PowerPeriodDto{Period = 0, Volume = 4}, new PowerPeriodDto{Period = 1, Volume = 4}, new PowerPeriodDto{Period = 2, Volume = 43}}},

            }; 

            var tradeProcessorService = new TradeProcessorService(tradeDataService.Object, exportService.Object, dateTimeService.Object);
            var aggregatedPowerDtos = tradeProcessorService.GetAggregatedPowerDtos(powerTradeDtos);
            Assert.AreEqual(3, aggregatedPowerDtos.Count);
        }
    }
}
