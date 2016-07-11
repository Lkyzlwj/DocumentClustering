/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/5/23
** Last Modify: 2016/5/23
** desc：       日期与时间的通用工具类
** Ver.:        V0.1.0

************************************************************************/

using System;

namespace org.common.utils.time
{
    public class DateTimeUtils
    {
        /// <summary>
        /// 将 Unix 时间戳转换为 DateTime 类型时间
        /// </summary>
        /// <param name="millis"> Unix 时间戳 </param>
        /// <returns>DateTime</returns>
        public static DateTime convertLongDateTime(long millis)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddSeconds(millis);
            return time;
        }

        /// <summary>
        /// 将 c# DateTime 时间格式转换为 Unix 时间戳格式
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>Unix 时间戳</returns>
        public static long convertDateTimeLong(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long millis = (dateTime.Ticks - startTime.Ticks) / 10000; //除10000调整为13位
            return millis;
        }

        /// <summary>
        /// 获得当前时间的 Unix 时间戳
        /// </summary>
        /// <returns>Unix 时间戳</returns>
        public static long getCurrentTimeStamp()
        {
            return convertDateTimeLong(DateTime.Now);
        }

        /// <summary>
        /// 获得当地凌晨的时间戳（含时区）
        /// </summary>
        /// <returns></returns>
        public static long getBeforeDawnTimeStamp()
        {
            DateTime today = DateTime.Now;
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0));
            return convertDateTimeLong(dateTime);
        }

        /// <summary>
        /// 获得当地凌晨的时间戳（不含时区）
        /// </summary>
        /// <returns></returns>
        public static long getBeforeDawnTimeStampNoZone()
        {
            DateTime today = DateTime.Now;
            return convertDateTimeLong(new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0));
        }
    }
}
