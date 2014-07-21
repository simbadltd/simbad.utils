using System;

namespace Simbad.Utils.Extenders
{
    public static class TimeZoneExtender
    {
        public static DateTime ToUtc(this DateTime local, TimeZoneInfo tz)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(local, tz.Id, "UTC");
        }

        public static DateTime ToLocal(this DateTime utc, TimeZoneInfo tz)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utc, "UTC", tz.Id);
        }
    }
}