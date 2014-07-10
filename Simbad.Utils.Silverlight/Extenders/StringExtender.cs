using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Simbad.Utils.Extenders
{
    public static class StringExtender
    {
        public static int ToInt(this object d, int defaultValue = 0, CultureInfo cultureInfo = null)
        {
            var convertible = d as IConvertible;

            if (convertible == null)
            {
                return defaultValue;
            }

            return convertible.ToInt32((cultureInfo ?? CultureInfo.InvariantCulture).NumberFormat);
        }

        public static int ToInt(this string str, int defaultValue = 0, CultureInfo cultureInfo = null)
        {
            int i;
            return int.TryParse(str, NumberStyles.Any, cultureInfo ?? CultureInfo.InvariantCulture, out i) ? i : defaultValue;
        }

        public static Int64 ToInt64(this object d, Int64 defaultValue = 0, CultureInfo cultureInfo = null)
        {
            var convertible = d as IConvertible;

            if (convertible == null)
            {
                return defaultValue;
            }

            return convertible.ToInt64((cultureInfo ?? CultureInfo.InvariantCulture).NumberFormat);
        }

        public static Int64 ToInt64(this string str, Int64 defaultValue = 0, CultureInfo cultureInfo = null)
        {
            Int64 i;
            return Int64.TryParse(str, NumberStyles.Any, cultureInfo ?? CultureInfo.InvariantCulture, out i) ? i : defaultValue;
        }

        public static double ToDouble(this object d, double defaultValue = 0d, CultureInfo cultureInfo = null)
        {
            var convertible = d as IConvertible;

            if (convertible == null)
            {
                return defaultValue;
            }

            return convertible.ToDouble((cultureInfo ?? CultureInfo.InvariantCulture).NumberFormat);
        }

        public static double ToDouble(this string str, double defaultValue = 0d, CultureInfo cultureInfo = null)
        {
            double d;
            return double.TryParse(str, NumberStyles.Any, cultureInfo ?? CultureInfo.InvariantCulture, out d) ? d : defaultValue;
        }

        public static bool ToBoolean(this object d, bool defaultValue = false, CultureInfo cultureInfo = null)
        {
            var convertible = d as IConvertible;

            if (convertible == null)
            {
                return defaultValue;
            }

            return convertible.ToBoolean((cultureInfo ?? CultureInfo.InvariantCulture).NumberFormat);
        }

        public static bool ToBoolean(this string str, bool defaultValue = false)
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
