using System;

namespace Simbad.Utils.Extenders
{
    public static class DateTimeExtender
    {
        public static bool IsThisYear(this DateTime dt, int year)
        {
            var result = dt.Year == year;
            return result;
        }

        public static bool IsThisMonth(this DateTime dt, int year, int month)
        {
            var result = dt.Month == month && dt.Year == year;
            return result;
        }

        public static bool IsThisDay(this DateTime dt, int year, int month, int day)
        {
            var result = dt.Day == day && dt.Month == month && dt.Year == year;
            return result;
        }

        public static DateTime CutMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        public static DateTime CutSeconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, 0);
        }

        public static DateTime CutMinutes(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, 0);
        }

        public static DateTime CutHours(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime ChangeMillisecond(this DateTime dt, int millisecond)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, millisecond);
        }

        public static DateTime ChangeSecond(this DateTime dt, int second)
        {
            return ChangeSecond(dt, second, dt.Millisecond);
        }

        public static DateTime ChangeSecond(this DateTime dt, int second, int millisecond)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, second, millisecond);
        }

        public static DateTime ChangeMinute(this DateTime dt, int minute)
        {
            return ChangeMinute(dt, minute, dt.Second, dt.Millisecond);
        }

        public static DateTime ChangeMinute(this DateTime dt, int minute, int second, int millisecond)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, minute, second, millisecond);
        }

        public static DateTime ChangeHour(this DateTime dt, int hour)
        {
            return ChangeHour(dt, hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        public static DateTime ChangeHour(this DateTime dt, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, hour, minute, second, millisecond);
        }

        public static DateTime ChangeDay(this DateTime dt, int day)
        {
            return ChangeDay(dt, day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        public static DateTime ChangeDay(this DateTime dt, int day, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(dt.Year, dt.Month, day, hour, minute, second, millisecond);
        }

        public static DateTime ChangeMonth(this DateTime dt, int month)
        {
            return ChangeMonth(dt, month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        public static DateTime ChangeMonth(this DateTime dt, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(dt.Year, month, day, hour, minute, second, millisecond);
        }

        public static DateTime ChangeYear(this DateTime dt, int year)
        {
            return new DateTime(year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        public static bool IsBetween(this DateTime dt, int h1, int m1, int h2, int m2)
        {
            var h = dt.Hour;
            var m = dt.Minute;
            return (h > h1 || (h == h1 && m >= m1)) && (h < h2 || (h == h2 && m < m2));
        }
    }
}