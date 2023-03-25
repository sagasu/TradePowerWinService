namespace TradePowerWinService.Services
{
    public interface IDateTimeService
    {
        DateTime GetDateTime ();
        DateTime Parse(string dateToParse);
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime GetDateTime () => DateTime.Now;
        public DateTime Parse(string dateToParse) => DateTime.Parse(dateToParse);
    }
}
