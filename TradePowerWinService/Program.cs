using Services;
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
        services.AddHostedService<TimedService>();
        services.Configure<ServiceConfig>(context.Configuration.GetSection(ServiceConfig.ServiceConfigName));
    }).UseWindowsService()

    .Build();


await host.RunAsync();
