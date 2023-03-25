using Services;
using TradePowerWinService;
using TradePowerWinService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services=>
    {
        services.AddTransient<IPowerService, PowerService>();
        services.AddTransient<ITradeDataService, TradeDataService>();
        services.AddTransient<ITradeProcessorService, TradeProcessorService>();
        services.AddTransient<IExportService, ExportService>();
        services.AddHostedService<Worker>();
        services.AddHostedService<TimedService>();
    })
    .Build();

await host.RunAsync();
