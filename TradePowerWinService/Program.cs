using Services;
using TradePowerWinService.Config;
using TradePowerWinService.Services;
using Microsoft.Extensions.Logging.EventLog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services )=>
    {
        services.AddTransient<IPowerService, PowerService>();
        services.AddTransient<ITradeDataService, TradeDataService>();
        services.AddTransient<ITradeProcessorService, TradeProcessorService>();
        services.AddTransient<IExportService, ExportService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddHostedService<TimedService>();
        services.Configure<ServiceConfig>(context.Configuration.GetSection(ServiceConfig.ServiceConfigName));
    })
    .ConfigureLogging(logging => logging.AddEventLog())
    .UseWindowsService()

    .Build();


await host.RunAsync();
