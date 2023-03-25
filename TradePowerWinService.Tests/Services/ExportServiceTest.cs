using Microsoft.Extensions.Configuration;
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
            var mockSection = new Mock<IConfigurationSection>();
            var path = "C:\\Users\\mattk\\AppData\\Local\\Temp";
            mockSection.Setup(x => x.Value).Returns(path);

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("ExportPath")).Returns(mockSection.Object);

            var date = DateTime.Now;
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(date);

            var serviceConfig = new ServiceConfig() { ExportPath = path };
            var option = new Mock<IOptions<ServiceConfig>>();
            option.Setup(x => x.Value).Returns(serviceConfig);
            
            new ExportService(configuration.Object, dateTimeService.Object, option.Object).Export(new List<PowerTradeExportDTO>());
            var exportedFilePath = ExportService.GetExportedFilePath(path,ExportService.GetExportFileName(date));
            Assert.IsTrue(File.Exists(exportedFilePath));
        }
    }
}