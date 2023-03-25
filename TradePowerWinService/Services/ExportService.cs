using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using TradePowerWinService.Models;

namespace TradePowerWinService.Services
{
    public interface IExportService
    {
        void Export(IEnumerable<PowerTradeExportDTO> powerTrades);
    }

    public class ExportService : IExportService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;

        public ExportService(IConfiguration configuration, IDateTimeService dateTimeService)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
        }

        public static string GetExportFileName (DateTime date) => $"PowerPosition_{date.ToString("YYYYMMDD_HHMM")}.csv";
        public void Export(IEnumerable<PowerTradeExportDTO> powerTrades)
        {
            var date = _dateTimeService.GetDateTime();
            var path = $"{_configuration["ExportPath"]}\\{GetExportFileName(date)}";

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(powerTrades);
            }
        }
    }
}
