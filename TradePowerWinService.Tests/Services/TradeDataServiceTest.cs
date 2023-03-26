using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using TradePowerWinService.Config;
using TradePowerWinService.Services;

namespace TradePowerWinService.Tests.Services
{
    [TestClass]
    public class TradeDataServiceTest
    {
        [TestMethod]
        public void GetTradeData_PowerTradeProvided_ConversionToPowerTradeDtoSucceeded()
        {
            var date = DateTime.Now;
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(date);

            var powerTrades = Enumerable.Range(0, 4).Select(x => PowerTrade.Create(date, 3));

            var powerService = new Mock<IPowerService>();
            powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).Returns(new Task<IEnumerable<PowerTrade>>((() => powerTrades)));

            var logger = new Mock<ILogger<TradeDataService>>();

            var tradeDataService = new TradeDataService(logger.Object, powerService.Object, dateTimeService.Object);
            var trades = tradeDataService.GetPowerTrades();
            Assert.IsNotNull(trades);
        }
    }
}
