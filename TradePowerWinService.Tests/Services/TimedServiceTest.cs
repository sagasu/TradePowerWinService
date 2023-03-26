using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TradePowerWinService.Config;
using TradePowerWinService.Services;

namespace TradePowerWinService.Tests.Services
{
    [TestClass]
    public class TimedServiceTest
    {
        [TestMethod]
        public void ProcessTrade_NoExtraContext_ProcessTradeIsBeingCalled()
        {
            var tradeProcessorService = new Mock<ITradeProcessorService>();
            tradeProcessorService.Setup(x => x.ProcessTrade()).Returns(Task.CompletedTask);
            var processTrade = GetProcessTrade(tradeProcessorService);
            var anyObject = new Object();

            processTrade.ProcessTrade(anyObject);

            tradeProcessorService.Verify(x => x.ProcessTrade(), Times.Once);
        }

        private static TimedService GetProcessTrade(Mock<ITradeProcessorService> tradeProcessorService)
        {
            var serviceConfig = new ServiceConfig();
            var option = new Mock<IOptions<ServiceConfig>>();
            option.Setup(x => x.Value).Returns(serviceConfig);

            var logger = new Mock<ILogger<TimedService>>();

            var processTrade = new TimedService(logger.Object, tradeProcessorService.Object, option.Object);
            return processTrade;
        }
    }
}
