using System.Globalization;

public static class DateHelper
{
    private static readonly TimeZoneInfo IranTimeZone = GetIranTimeZone();

    // Transaction timestamps are stored as UTC. Database DateTime values may have
    // an Unspecified Kind, so treat their ticks as UTC before converting.
    public static DateTime ToIranTime(this DateTime utcDateTime)
    {
        var utcValue = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utcValue, IranTimeZone);
    }

    public static string ToPersianDate(this DateTime utcDateTime)
    {
        var iranDateTime = utcDateTime.ToIranTime();
        var persianCalendar = new PersianCalendar();

        return $"{persianCalendar.GetYear(iranDateTime)}/" +
               $"{persianCalendar.GetMonth(iranDateTime):00}/" +
               $"{persianCalendar.GetDayOfMonth(iranDateTime):00}";
    }

    private static TimeZoneInfo GetIranTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
        }
    }
}
