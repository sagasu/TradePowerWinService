using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Options;
using TradePowerWinService.Models;
using TradePowerWinService.Config;

namespace TradePowerWinService.Services
{
    public interface IExportService
    {
        void Export(IEnumerable<AggregatedPowerDto> aggregatedPowerDtos);
    }

    public class ExportService : IExportService
    {
        private readonly ILogger<ExportService> _logger;
        private readonly IDateTimeService _dateTimeService;
        private readonly ServiceConfig _serviceConfig;
        private const string EXPORT_START_MESSAGE = "Export to file starting.";
        private const string EXPORT_SUCCESS_MESSAGE = "Export to file succeeded.";
        public ExportService(ILogger<ExportService> logger, IDateTimeService dateTimeService, IOptions<ServiceConfig> serviceConfig)
        {
            _logger = logger;
            _dateTimeService = dateTimeService;
            _serviceConfig = serviceConfig.Value;
        }

        public static string GetExportFileName (DateTime date) => $"PowerPosition_{date.ToString("yyyyMMdd_HHmm")}.csv";
        public static string GetExportFilePath (string? path, string fileName) => Path.Combine(path ?? string.Empty, fileName);

        public void Export(IEnumerable<AggregatedPowerDto> aggregatedPowerDtos)
        {
            var date = _dateTimeService.GetDateTime();
            var path = GetExportFilePath(_serviceConfig.ExportPath, GetExportFileName(date));

            _logger.LogInformation(EXPORT_START_MESSAGE);
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(aggregatedPowerDtos);
            }
            _logger.LogInformation(EXPORT_SUCCESS_MESSAGE);
        }
    }
}
