using Microsoft.Extensions.DependencyInjection;
using Services;
using TradePowerWinService;
using TradePowerWinService.Config;
using TradePowerWinService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services )=>
    {
        services.AddTransient<IPowerService, PowerService>();
        services.AddTransient<ITradeDataService, TradeDataService>();
        services.AddTransient<ITradeProcessorService, TradeProcessorService>();
        services.AddTransient<IExportService, ExportService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddHostedService<Worker>();
        services.AddHostedService<TimedService>();
        services.Configure<ServiceConfig>(context.Configuration.GetSection(ServiceConfig.ServiceConfigName));
    })

    .Build();


await host.RunAsync();
