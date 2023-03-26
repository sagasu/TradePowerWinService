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
        private const int ANY_NUMBER = 10;

        [TestMethod]
        public void GetAggregatedPowerDtos_TwoTradesTwoPeriods_FirstPeriodVolumeAggregatedCorrectly()
        {
            var firstTradeFirstPeriodVolume = 3;
            var secondTradeFirstPeriodVolume = 4;
            var powerTradeDtos = GetPowerTradeDtos(firstTradeFirstPeriodVolume: firstTradeFirstPeriodVolume, secondTradeFirstPeriodVolume: secondTradeFirstPeriodVolume);
            var tradeProcessorService = GetTradeProcessorService();
            
            var aggregatedPowerDtos = tradeProcessorService.GetAggregatedPowerDtos(powerTradeDtos);

            Assert.AreEqual(firstTradeFirstPeriodVolume + secondTradeFirstPeriodVolume, aggregatedPowerDtos[0].Volume);
        }
        
        [TestMethod]
        public void GetAggregatedPowerDtos_TwoTradesTwoPeriods_SecondPeriodVolumeAggregatedCorrectly()
        {
            var firstTradeSecondPeriodVolume = 5;
            var secondTradeSecondPeriodVolume = 6;
            var powerTradeDtos = GetPowerTradeDtos(firstTradeSecondPeriodVolume: firstTradeSecondPeriodVolume, secondTradeSecondPeriodVolume: secondTradeSecondPeriodVolume);
            var tradeProcessorService = GetTradeProcessorService();
            

            var aggregatedPowerDtos = tradeProcessorService.GetAggregatedPowerDtos(powerTradeDtos);

            Assert.AreEqual(firstTradeSecondPeriodVolume + secondTradeSecondPeriodVolume, aggregatedPowerDtos[1].Volume);
        }

        private static List<PowerTradeDto> GetPowerTradeDtos(int firstTradeFirstPeriodVolume = ANY_NUMBER, int firstTradeSecondPeriodVolume = ANY_NUMBER, int secondTradeFirstPeriodVolume = ANY_NUMBER, int secondTradeSecondPeriodVolume = ANY_NUMBER)
        {
            var anyDate = DateTime.Now;
            var powerTradeDtos = new List<PowerTradeDto>
            {
                new PowerTradeDto
                {
                    Date = anyDate,
                    Periods = new List<PowerPeriodDto>
                    {
                        new PowerPeriodDto { Period = 1, Volume = firstTradeFirstPeriodVolume },
                        new PowerPeriodDto { Period = 2, Volume = firstTradeSecondPeriodVolume }
                    }
                },
                new PowerTradeDto
                {
                    Date = anyDate,
                    Periods = new List<PowerPeriodDto>
                    {
                        new PowerPeriodDto { Period = 1, Volume = secondTradeFirstPeriodVolume },
                        new PowerPeriodDto { Period = 2, Volume = secondTradeSecondPeriodVolume }
                    }
                },
            };
            return powerTradeDtos;
        }

        private static TradeProcessorService GetTradeProcessorService()
        {
            var tradeDataService = new Mock<ITradeDataService>();
            var exportService = new Mock<IExportService>();
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.Parse(It.IsAny<string>())).Returns(DateTime.Parse(TradeProcessorService.START_TIME));
            var tradeProcessorService =
                new TradeProcessorService(tradeDataService.Object, exportService.Object, dateTimeService.Object);
            return tradeProcessorService;
        }
    }
}
