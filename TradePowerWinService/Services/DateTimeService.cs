namespace TradePowerWinService.Services
{
    public interface IDateTimeService
    {
        DateTime GetDateTime ();
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime GetDateTime () => DateTime.Now;
    }
}
