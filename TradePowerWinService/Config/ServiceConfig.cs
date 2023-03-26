namespace TradePowerWinService.Config
{
    public class ServiceConfig
    {
        public const string ServiceConfigName = "ServiceConfig";
        public string? ExportPath { get; set; }
        public int RunEveryMinutes { get; set; }
    }
}
