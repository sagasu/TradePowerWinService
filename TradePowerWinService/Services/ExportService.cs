using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace TradePowerWinService.Services
{
    public interface IExportService
    {
        void Export(IEnumerable<PowerTrade> powerTrades);
    }

    public class ExportService : IExportService
    {
        private readonly IConfiguration _configuration;
        public ExportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Export(IEnumerable<PowerTrade> powerTrades)
        {
            var date = DateTime.Now;
            // TODO: format can be extracted to config file.
            var fileName = $"PowerPosition_{date.ToString("YYYYMMDD_HHMM")}.csv";
            var path = $"{_configuration["ExportPath"]}\\${fileName}";

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(powerTrades);
            }
        }
    }
}
