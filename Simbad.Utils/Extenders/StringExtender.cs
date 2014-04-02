using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Simbad.Utils.Extenders
{
    public static class StringExtender
    {
        public static int ToInt(this string str)
        {
            return ToInt(str, 0, CultureInfo.InvariantCulture);
        }

        public static int ToInt(this string str, int defaultValue)
        {
            return ToInt(str, defaultValue, CultureInfo.InvariantCulture);
        }

        public static int ToInt(this string str, int defaultValue, CultureInfo cultureInfo)
        {
            int i;
            return int.TryParse(str, NumberStyles.Any, cultureInfo, out i) ? i : defaultValue;
        }

        public static Int64 ToInt64(this string str)
        {
            return ToInt64(str, 0, CultureInfo.InvariantCulture);
        }

        public static Int64 ToInt64(this string str, Int64 defaultValue)
        {
            return ToInt64(str, defaultValue, CultureInfo.InvariantCulture);
        }

        public static Int64 ToInt64(this string str, Int64 defaultValue, CultureInfo cultureInfo)
        {
            Int64 i;
            return Int64.TryParse(str, NumberStyles.Any, cultureInfo, out i) ? i : defaultValue;
        }

        public static double ToDouble(this string str)
        {
            return ToDouble(str, 0, CultureInfo.InvariantCulture);
        }

        public static double ToDouble(this string str, double defaultValue)
        {
            return ToDouble(str, defaultValue, CultureInfo.InvariantCulture);
        }

        public static double ToDouble(this string str, double defaultValue, CultureInfo cultureInfo)
        {
            double d;
            return double.TryParse(str, NumberStyles.Any, cultureInfo, out d) ? d : defaultValue;
        }

        public static bool ToBoolean(this string str)
        {
            return ToBoolean(str, false);
        }

        public static bool ToBoolean(this string str, bool defaultValue)
        {
            bool b;
            return bool.TryParse(str, out b) ? b : defaultValue;
        }

        public static DateTime ToDateTime(this string str)
        {
            return ToDateTime(str, DateTime.MinValue);
        }

        public static DateTime ToDateTime(this string str, DateTime defaultDateTime)
        {
            return ToDateTime(str, defaultDateTime, CultureInfo.InvariantCulture);
        }

        public static DateTime ToDateTime(this string str, DateTime defaultDateTime, CultureInfo cultureInfo)
        {
            DateTime d;
            return DateTime.TryParse(str, cultureInfo, DateTimeStyles.None, out d) ? d : defaultDateTime;
        }

        public static MemoryStream ToMemoryStream(this string str)
        {
            return ToMemoryStream(str, new UTF8Encoding());
        }

        public static MemoryStream ToMemoryStream(this string str, Encoding encoding)
        {
            var bytes = encoding.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
