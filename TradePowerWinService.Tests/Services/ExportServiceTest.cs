using Microsoft.Extensions.Configuration;
using Moq;
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
            
            new ExportService(configuration.Object, dateTimeService.Object).Export(new List<PowerTradeExportDTO>());
            var exportedFilePath = $"{path}\\{ExportService.GetExportFileName(date)}";
            Assert.IsTrue(File.Exists(exportedFilePath));
        }
    }
}