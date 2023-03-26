using Microsoft.Extensions.Logging;
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
            var anyExportPath = Path.GetTempPath();
            var anyDate = DateTime.Now;
            var exportService = GetExportService(anyDate, anyExportPath);

            exportService.Export(new List<AggregatedPowerDto>());

            var exportedFilePath = ExportService.GetExportFilePath(anyExportPath, ExportService.GetExportFileName(anyDate));
            Assert.IsTrue(File.Exists(exportedFilePath));
        }

        private static ExportService GetExportService(DateTime anyDate, string exportPath)
        {
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(anyDate);

            var serviceConfig = new ServiceConfig() { ExportPath = exportPath };
            var option = new Mock<IOptions<ServiceConfig>>();
            option.Setup(x => x.Value).Returns(serviceConfig);

            var logger = new Mock<ILogger<ExportService>>();

            var exportService = new ExportService(logger.Object, dateTimeService.Object, option.Object);
            return exportService;
        }
    }
}