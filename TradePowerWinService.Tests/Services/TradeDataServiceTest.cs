using Microsoft.Extensions.Logging;
using Moq;
using Services;
using TradePowerWinService.Services;

namespace TradePowerWinService.Tests.Services
{
    [TestClass]
    public class TradeDataServiceTest
    {
        private const int ANY_NUMBER = 10;

        [TestMethod]
        public async Task GetTradeData_PowerTradeProvided_AllPowerTradesReturned()
        {
            var numberOfPowerTrades = 4;
            var tradeDataService = GetTradeDataService(numberOfPowerTrades: numberOfPowerTrades);

            var trades = await tradeDataService.GetPowerTrades();

            Assert.AreEqual(numberOfPowerTrades, trades.Count());
        }

        [TestMethod]
        public async Task GetTradeData_PowerPeriodsProvided_AllPowerPeriodsReturned()
        {
            var numberOfPowerPeriods = 3;
            var tradeDataService = GetTradeDataService(numberOfPowerPeriods: numberOfPowerPeriods);

            var trades = await tradeDataService.GetPowerTrades();

            Assert.AreEqual(numberOfPowerPeriods, trades.First().Periods.Count());
        }

        private static TradeDataService GetTradeDataService(int numberOfPowerTrades = ANY_NUMBER, int numberOfPowerPeriods = ANY_NUMBER)
        {
            var anyDate = DateTime.Now;
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(anyDate);

            var powerTrades = Enumerable.Range(0, numberOfPowerTrades).Select(x => PowerTrade.Create(anyDate, numberOfPowerPeriods));

            var powerService = new Mock<IPowerService>();
            powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(powerTrades);

            var logger = new Mock<ILogger<TradeDataService>>();

            var tradeDataService = new TradeDataService(logger.Object, powerService.Object, dateTimeService.Object);
            return tradeDataService;
        }

        
    }
}
