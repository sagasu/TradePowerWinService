﻿using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Services;
using TradePowerWinService.Models;
using TradePowerWinService.Config;

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
        private readonly ServiceConfig _serviceConfig;

        public ExportService(IConfiguration configuration, IDateTimeService dateTimeService, IOptions<ServiceConfig> serviceConfig)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _serviceConfig = serviceConfig.Value;
        }

        public static string GetExportFileName (DateTime date) => $"PowerPosition_{date.ToString("YYYYMMDD_HHMM")}.csv";
        public static string GetExportedFilePath (string path, string fileName) => Path.Combine(path, fileName);

        public void Export(IEnumerable<PowerTradeExportDTO> powerTrades)
        {
            var date = _dateTimeService.GetDateTime();
            var path = GetExportedFilePath(_serviceConfig.ExportPath, GetExportFileName(date));
            
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(powerTrades);
            }
        }
    }
}