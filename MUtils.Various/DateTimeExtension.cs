using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUtils.Various
{
    public static class DateTimeExtension
    {
        private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>Convert a long into a DateTime</summary>
        public static DateTime FromUnixTime(this long unixTime)
        {
            DateTime dateTime = DateTimeExtension.UnixStart;
            dateTime = dateTime.AddSeconds((double)unixTime);
            return dateTime.ToLocalTime();
        }

        public static DateTime FromUnixMillisecondsTime(this long unixTime)
        {
            DateTime dateTime = DateTimeExtension.UnixStart;
            dateTime = dateTime.AddMilliseconds((double)unixTime);
            return dateTime.ToLocalTime();
        }

        /// <summary>Convert a DateTime into a long</summary>
        /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
        /// <exception cref="T:System.OverflowException"></exception>
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return 0;
            double totalSeconds = (dateTime.ToUniversalTime() - DateTimeExtension.UnixStart).TotalSeconds;
            double num = 0.0;
            if (totalSeconds < num)
                throw new ArgumentOutOfRangeException(nameof(dateTime), "Unix epoch starts January 1st, 1970");
            return Convert.ToInt64(totalSeconds);
        }

        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return 0;
            double totalMilliSeconds = (dateTime.ToUniversalTime() - DateTimeExtension.UnixStart).TotalMilliseconds;
            double num = 0.0;
            if (totalMilliSeconds < num)
                throw new ArgumentOutOfRangeException(nameof(dateTime), "Unix epoch starts January 1st, 1970");
            return Convert.ToInt64(totalMilliSeconds);
        }

        internal static string EncodeUtf8(this string value)
        {
            return string.Join<char>(string.Empty, ((IEnumerable<byte>)Encoding.UTF8.GetBytes(value)).Select<byte, char>(new Func<byte, char>(Convert.ToChar)));
        }
    }
}
