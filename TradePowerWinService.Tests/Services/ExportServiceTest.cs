using Microsoft.Extensions.Options;
using Moq;
using TradePowerWinService.Config;
using TradePowerWinService.Models;
using TradePowerWinService.Services;

namespace TradePowerWinService.Tests.Services
{
    [TestClass]
    public class ExportServiceTest
    {
        [TestMethod]
        public void Export_DataProvided_FileExported()
        {
            var exportPath = "C:\\Users\\mattk\\AppData\\Local\\Temp";

            var date = DateTime.Now;
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(date);

            var serviceConfig = new ServiceConfig() { ExportPath = exportPath };
            var option = new Mock<IOptions<ServiceConfig>>();
            option.Setup(x => x.Value).Returns(serviceConfig);
            
            new ExportService(dateTimeService.Object, option.Object).Export(new List<AggregatedPowerDto>());
            var exportedFilePath = ExportService.GetExportFilePath(exportPath,ExportService.GetExportFileName(date));

            Assert.IsTrue(File.Exists(exportedFilePath));
        }
    }
}