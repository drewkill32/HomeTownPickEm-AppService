using System;

namespace HomeTownPickEm.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset GetLastThusMidnight(this DateTimeOffset dt)
        {
            var lastThurs = dt;
            while (lastThurs.DayOfWeek != DayOfWeek.Thursday)
            {
                lastThurs = lastThurs.AddDays(-1);
            }

            return new DateTimeOffset(lastThurs.Date.AddHours(4));
        }
    }
}