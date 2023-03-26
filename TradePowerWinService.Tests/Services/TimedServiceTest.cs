using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradePowerWinService.Config;
using TradePowerWinService.Services;

namespace TradePowerWinService.Tests.Services
{
    [TestClass]
    public class TimedServiceTest
    {
        [TestMethod]
        public void DoWork_NoExtraContext_ProcessTradeIsBeingCalled()
        {
            var tradeProcessorService = new Mock<ITradeProcessorService>();
            tradeProcessorService.Setup(x => x.ProcessTrade()).Returns(new Task(() => {}));

            var serviceConfig = new ServiceConfig();
            var option = new Mock<IOptions<ServiceConfig>>();
            option.Setup(x => x.Value).Returns(serviceConfig);

            var logger = new Mock<ILogger<TimedService>>();

            var processTrade = new TimedService(logger.Object, tradeProcessorService.Object, option.Object);
            var anyObject = new Object();
            processTrade.DoWork(anyObject);

            tradeProcessorService.Verify(x => x.ProcessTrade(), Times.Once);
        }
    }
}
