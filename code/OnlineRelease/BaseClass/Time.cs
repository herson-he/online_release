using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRelease.BaseClass
{
    public class Time
    {
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static uint ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = new System.DateTime(1970, 1, 1);
            intResult = (time - startTime).TotalSeconds;
            return (uint)intResult;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvrtToDateTimeFormMilliseconds(uint d)
        {
            DateTime time = System.DateTime.MinValue;
            DateTime startTime = new System.DateTime(1970, 1, 1);
            time = startTime.AddMilliseconds(d);
            return time;
        }
    }
}
