using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public class Time
    {
        /// <summary>
        /// 获取下一个月
        /// </summary>
        /// <returns></returns>
        public static int GetNextMonth(int monthID)
        {
            DateTime dt = Convert.ToDateTime(monthID.ToString().Insert(4, "-") + "-01");
            return Convert.ToInt32(dt.AddMonths(1).ToString("yyyyMM"));
        }

        /// <summary>
        /// 获取上一个月
        /// </summary>
        /// <returns></returns>
        public static int GetLastMonth(int monthID)
        {
            DateTime dt = Convert.ToDateTime(monthID.ToString().Insert(4, "-") + "-01");
            return Convert.ToInt32(dt.AddMonths(-1).ToString("yyyyMM"));
        }

        /// <summary>
        /// 获取月总天数
        /// </summary>
        /// <param name="monthID"></param>
        /// <returns></returns>
        public static int GetMonthDays(int monthID)
        {
            DateTime startDt = Convert.ToDateTime(monthID.ToString().Insert(4, "-") + "-01");
            DateTime endDt = startDt.AddMonths(1).AddDays(-1);
            int count = 0;
            while (true)
            {
                if (startDt > endDt)
                    break;
                count++;
                startDt = startDt.AddDays(1);
            }
            return count;
        }

        /// <summary>
        /// 获取本周的开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetStartDateThisWeek()
        {
            int num = Convert.ToInt32(DateTime.Now.DayOfWeek);
            return DateTime.Now.Date.AddDays(0 - num);
        }

        /// <summary>
        /// 获取上周的开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetStartDatePreWeek()
        {
            return GetStartDateThisWeek().AddDays(-7);
        }

        /// <summary>
        /// 获取上周的结束时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetEndDatePreWeek()
        {
            return Convert.ToDateTime(GetStartDateThisWeek().AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59.999");
        }

        /// <summary>
        /// 获取月开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthStartDate(DateTime dt)
        {
            return dt.AddDays(1 - dt.Day).Date;
        }

        /// <summary>
        /// 获取月结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthEndDate(DateTime dt)
        {
            return dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1).Date;
        }

        /// <summary>
        /// 获取传入的时间离当前时间的年月数
        /// </summary>
        /// <param name="objDate"></param>
        /// <returns></returns>
        public static string GetYearAndMonthLength(object objDate)
        {
            DateTime date = Convert.ToDateTime(objDate);

            int monthCount = (DateTime.Now.Year - date.Year) * 12 + (DateTime.Now.Month - date.Month) + (DateTime.Now.Day < date.Day ? -1 : 0);
            int years = monthCount / 12;
            int month = monthCount % 12;

            StringBuilder returnSB = new StringBuilder();
            if (years > 0)
                returnSB.AppendFormat("{0}年", years);
            if (month > 0)
                returnSB.AppendFormat("{0}个月", month);
            string returnStr = returnSB.ToString();
            if (string.IsNullOrEmpty(returnStr))
                returnStr = "还未满1个月";
            return returnStr;
        }

        /// <summary>
        /// 获取时间段内的工作日数
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <returns></returns>
        public static int GetWorkDay(DateTime startDT, DateTime endDT)
        {
            int days = Convert.ToInt32((endDT.Date - startDT.Date).TotalDays) + 1;
            int workDays = 0;
            if (days == 0)
                return workDays;
            for (int i = 0; i < days; i++)
            {
                DateTime currentDT = endDT.AddDays(i);
                if (currentDT.DayOfWeek != DayOfWeek.Sunday && currentDT.DayOfWeek != DayOfWeek.Saturday)
                    workDays++;
            }
            return workDays;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = System.DateTime.MinValue;
            DateTime startTime = new System.DateTime(1970, 1, 1);
            time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static double ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = new System.DateTime(1970, 1, 1);
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvrtToDateTimeFormMilliseconds(double d)
        {
            DateTime time = System.DateTime.MinValue;
            DateTime startTime = new System.DateTime(1970, 1, 1);
            time = startTime.AddMilliseconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static double ConvertToMillisecondsFromDateTime(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = new System.DateTime(1970, 1, 1);
            intResult = (time - startTime).TotalMilliseconds;
            return intResult;
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <returns></returns>
        public static string FormatDateSpan(object date)
        {
            DateTime time = (DateTime)date;
            TimeSpan span = (TimeSpan)(DateTime.Now - time);
            if (span.TotalDays >= 365.0)
                return string.Format("{0} 年前", (int)(span.TotalDays / 365.0));

            if (span.TotalDays >= 30.0)
                return string.Format("{0} 个月前", (int)(span.TotalDays / 30.0));

            if (span.TotalDays >= 7.0)
                return string.Format("{0} 周前", (int)(span.TotalDays / 7.0));

            if (span.TotalDays >= 1.0)
                return string.Format("{0} 天前", (int)span.TotalDays);

            if (span.TotalHours >= 1.0)
                return string.Format("{0} 小时前", (int)span.TotalHours);

            if (span.TotalMinutes >= 1.0)
                return string.Format("{0} 分钟前", (int)span.TotalMinutes);

            if (span.TotalDays <= -365.0)
                return string.Format("{0} 年后", (int)(Math.Abs(span.TotalDays) / 365.0));

            if (span.TotalDays <= -30.0)
                return string.Format("{0} 个月后", (int)(Math.Abs(span.TotalDays) / 30.0));

            if (span.TotalDays <= -30.0)
                return string.Format("{0} 周后", (int)(Math.Abs(span.TotalDays) / 7.0));

            if (span.TotalDays <= -1.0)
                return string.Format("{0} 天后", (int)(Math.Abs(span.TotalDays)));

            if (span.TotalHours <= -1.0)
                return string.Format("{0} 小时后", (int)(Math.Abs(span.TotalHours)));

            if (span.TotalMinutes <= -1.0)
                return string.Format("{0} 分钟后", (int)(Math.Abs(span.TotalMinutes)));

            return "刚刚";
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <returns></returns>
        public static string FormatDateSpanDays(object date)
        {
            DateTime time = (DateTime)date;
            TimeSpan span = (TimeSpan)(DateTime.Now.Date - time.Date);
            if (span.TotalDays >= 365.0)
                return string.Format("{0} 年前", (int)(span.TotalDays / 365.0));

            if (span.TotalDays >= 30.0)
                return string.Format("{0} 个月前", (int)(span.TotalDays / 30.0));

            if (span.TotalDays >= 7.0)
                return string.Format("{0} 周前", (int)(span.TotalDays / 7.0));

            if (span.TotalDays >= 1.0)
                return string.Format("{0} 天前", (int)span.TotalDays);

            if (span.TotalDays == 0)
                return string.Format("今天");

            if (span.TotalDays <= -365.0)
                return string.Format("{0} 年后", (int)(Math.Abs(span.TotalDays) / 365.0));

            if (span.TotalDays <= -30.0)
                return string.Format("{0} 个月后", (int)(Math.Abs(span.TotalDays) / 30.0));

            if (span.TotalDays <= -30.0)
                return string.Format("{0} 周后", (int)(Math.Abs(span.TotalDays) / 7.0));

            if (span.TotalDays <= -1.0)
                return string.Format("{0} 天后", (int)(Math.Abs(span.TotalDays)));

            return "今天";
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateSpan"></param>
        /// <returns></returns>
        public static string FormatLeaveTime(object date)
        {
            DateTime time = (DateTime)date;
            int days = Convert.ToInt32((DateTime.Now.Date - time.Date).TotalDays);
            if (days >= 365)
                return string.Format("{0} 年前", (int)(days / 365.0));

            if (days >= 30)
                return string.Format("{0} 月前", (int)(days / 30.0));

            if (days >= 7)
                return string.Format("{0} 周前", (int)(days / 7.0));

            if (days > 2)
                return string.Format("{0} 天前", days);

            if (days == 2)
                return "前天";

            if (days == 1)
                return "昨天";

            if (days == 0)
                return "今天";

            if (days == -1)
                return "明天";

            if (days == -2)
                return "后天";

            return string.Format("{0} 天后", Math.Abs(days));
        }

        /// <summary>
        /// 获取日期ID
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDateID(DateTime dt)
        {
            return Convert.ToInt32((dt.Date - Convert.ToDateTime("1900-01-01 00:00:00")).TotalDays);
        }

        /// <summary>
        /// 获取日期ID
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDateByDateID(int dateID)
        {
            return Convert.ToDateTime("1900-01-01 00:00:00").AddDays(dateID);
        }

        /// <summary>
        /// 获取月开始时间ID
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetMonthStartDateID(DateTime dt)
        {
            return GetDateID(GetMonthStartDate(dt));
        }

        /// <summary>
        /// 获取月结束时间ID
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetMonthEndDateID(DateTime dt)
        {
            return GetDateID(GetMonthEndDate(dt));
        }

        /// <summary>
        /// 通过周获取时间
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // 使用1月的第一个星期四来获得一年中的第一周
            // 它永远不会出现在第52/53周
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // 因为我们在第1周的日期增加了几天,
            // 我们需要减去1才能获得第1周的正确日期
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // 使用第一个星期四作为开始周确保我们在正确的一年开始
            // 然后我们添加周数乘以天数
            var result = firstThursday.AddDays(weekNum * 7);

            // 从星期四减去3天到星期一，这是ISO8601的第一个工作日
            return result.AddDays(-3);
        }

        /// <summary>
        /// 获取周算
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取当前周的周一Date
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
