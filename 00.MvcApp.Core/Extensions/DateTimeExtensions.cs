using System;

namespace MvcApp.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormat(this DateTime? dateTime, string format)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return dateTime.Value.ToString(format);
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToFullTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm");
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToFullTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return dateTime.Value.ToFullTimeString();
        }

        /// <summary>
        /// HH:mm
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
        
        public static DateTime ToSunday(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.Date;
            }
            return date.Date.AddDays(7 - (int) date.DayOfWeek);
        }

        public static DateTime ToMonthFirst(this DateTime date)
        {
            return date.AddDays(-date.Day + 1);
        }

        public static DateTime ToYearFirst(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime ToQuaterFirst(this DateTime date)
        {
            var month = date.Month / 3;
            if (date.Month % 3 == 0)
            {
                month = month - 1;
            }
            month = month * 3 + 1;
            return new DateTime(date.Year, month, 1);
        }

        public static long GetTime(this DateTime time)
        {
            return (long) (time - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static string GetWeekText(this DateTime datetime)
        {
            var date = datetime.Date;
            var thisSunday = DateTime.Today.ToSunday();
            var thisMonday = thisSunday.AddDays(-6);
            var lastMonday = thisMonday.AddDays(-7);
            var nextSunday = thisSunday.AddDays(7);
            if (date < lastMonday || date > nextSunday)
            {
                return null;
            }
            if (date < thisMonday)
            {
                return date.ToString("上ddd");
            }
            if (date > thisSunday)
            {
                return date.ToString("下ddd");
            }
            return date.ToString("本ddd");
        }

        /// <summary>
        /// 返回 今天12:21
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToTimeText2(this DateTime time)
        {
            if (time.Date == DateTime.Today)
            {
                return "今天 " + time.ToString("HH:mm");
            }
            if (time.Date == DateTime.Today.AddDays(-1))
            {
                return "昨天 " + time.ToString("HH:mm");
            }
            return time.ToString("yyyy-MM-dd");
        }

        public static int ToMonthNumber(this DateTime date)
        {
            return date.Year * 100 + date.Month;
        }

        public static int ToQuaterNumber(this DateTime date)
        {
            return date.Year * 10 + (date.Month % 3 == 0 ? date.Month / 3 : date.Month / 3 + 1);
        }

        public static int ToWeekNumber(this DateTime date)
        {
            var sunday = date.ToSunday();
            return sunday.ToDateNumber();
        }
        
        public static int ToYearWeekNumber(this DateTime date)
        {
            var firstSunday = new DateTime(date.Year, 1, 1).ToSunday();
            var d = date;
            if (firstSunday >= date)
            {
                d = date.ToSunday().AddDays(-6);
                firstSunday = new DateTime(d.Year, 1, 1).ToSunday();
            }
            var days = (int) ((d - firstSunday).TotalDays);
            var weeks = days / 7;
            if (days % 7 > 0)
            {
                weeks++;
            }
            return weeks;
        }

        public static int ToDateNumber(this DateTime date)
        {
            return date.ToMonthNumber() * 100 + date.Day;
        }

        public static int ToHourNumber(this DateTime date)
        {
            return date.ToDateNumber() * 100 + date.Hour;
        }

        public static long GetVersion(this DateTime time)
        {
            return (long) (time.AddHours(-8) - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static DateTime? Max(this DateTime? time1, DateTime? time2)
        {
            if (time1 == null && time2 == null)
            {
                return null;
            }
            if (time1 != null && time2 != null)
            {
                return time1.Value > time2.Value ? time1 : time2;
            }
            return time1 == null ? time2 : time1;
        }

        public static DateTime? Min(this DateTime? time1, DateTime? time2)
        {
            if (time1 == null && time2 == null)
            {
                return null;
            }
            if (time1 != null && time2 != null)
            {
                return time1.Value > time2.Value ? time2 : time1;
            }
            return time1 == null ? time2 : time1;
        }
    }
}
