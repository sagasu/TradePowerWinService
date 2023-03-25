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
        private readonly IDateTimeService _dateTimeService;
        private readonly ServiceConfig _serviceConfig;

        public ExportService(IDateTimeService dateTimeService, IOptions<ServiceConfig> serviceConfig)
        {
            _dateTimeService = dateTimeService;
            _serviceConfig = serviceConfig.Value;
        }

        public static string GetExportFileName (DateTime date) => $"PowerPosition_{date.ToString("YYYYMMDD_HHMM")}.csv";
        public static string GetExportFilePath (string path, string fileName) => Path.Combine(path, fileName);

        public void Export(IEnumerable<AggregatedPowerDto> aggregatedPowerDtos)
        {
            var date = _dateTimeService.GetDateTime();
            var path = GetExportFilePath(_serviceConfig.ExportPath, GetExportFileName(date));
            
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(aggregatedPowerDtos);
            }
        }
    }
}
