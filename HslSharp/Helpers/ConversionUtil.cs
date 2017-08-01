using System;
using System.Text;

namespace HslSharp.Helpers
{
    internal static class ConversionUtil
    {
        /// <summary>
        /// The En1545 zero date, A.K.A. 1997-01-01.
        /// </summary>
        internal static readonly DateTimeOffset En1545ZeroDate = new DateTimeOffset(1997, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// Converts from En1545 (number of days since 1997-01-01) to a standard .NET DateTimeOffset.
        /// </summary>
        /// <param name="date">The date in En1545 format (days since 1997-01-01).</param>
        /// <returns>The .NET DateTimeOffet equivalent.</returns>
        internal static DateTimeOffset FromEn1545Date(ushort date)
        {
            return FromEn1545DateAndTime(date, 0);
        }

        /// <summary>
        /// Convert from En1545 (number of days since 1997-01-01, and number of minute since 00:00) to a standard .NET DateTimeOffset.
        /// </summary>
        /// <param name="date">The date in En1545 format (days since 1997-01-01).</param>
        /// <param name="time">The time in En1545 format (minutes since 00:00).</param>
        /// <returns>The .NET DateTimeOffet equivalent.</returns>
        internal static DateTimeOffset FromEn1545DateAndTime(ushort date, ushort time)
        {
            TimeSpan daysAndMinutes = TimeSpan.FromDays(date).Add(TimeSpan.FromMinutes(time));
            DateTimeOffset localDate = En1545ZeroDate + daysAndMinutes;
            TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(localDate);
            DateTimeOffset utcDate = localDate - utcOffset;
            return utcDate;
        }

        /// <summary>
        /// Converts an array of bytes into a hex string.
        /// </summary>        
        internal static string GetHexString(byte[] hexStringBytes)
        {
            var builder = new StringBuilder();
            for(int i = 0; i < hexStringBytes.Length; i++)
            {
                builder.Append(Convert.ToString((hexStringBytes[i] & 0xFF) + 0x100, 16).Substring(1));
            }

            return builder.ToString();
        }
    }
}
