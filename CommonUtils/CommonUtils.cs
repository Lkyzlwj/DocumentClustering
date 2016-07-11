/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016‎/‎5‎/‎20
** Last Modify: 2016/6/3
** desc：       通用工具类
** Ver.:        V0.1.1

************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using org.common.utils.str;
using org.common.utils.time;

namespace org.common.utils
{
    public class CommonUtils
    {
        /// <summary>
        /// 获得一个文件的完整的文件名，但不包括后缀名
        /// </summary>
        /// <param name="fileFullName">完整文件名</param>
        /// <returns>不包括后缀的文件名</returns>
        public static string getFileNameNoSuffix(string fileFullName)
        {
            if (StringUtils.isEmpty(fileFullName))
            {
                return string.Empty;
            }

            return spliceFileName(fileFullName);
        }

        /// <summary>
        /// 将一个文件名拆分再选择合并
        /// </summary>
        /// <param name="fileFullName">完整文件名</param>
        /// <returns>不包括后缀的文件名</returns>
        private static string spliceFileName(string fileFullName)
        {
            string merge = string.Empty;
            string[] fragments = fileFullName.Split('.');

            bool fristFlag = true;
            for (int index = 0; index < fragments.Length - 1; index++)
            {
                merge = String.Concat(merge, (fristFlag ? "" : ".") + fragments[index]);
                fristFlag = false;
            }

            return merge;
        }

        /// <summary>
        /// 计算某一个文件夹下以时间戳命名的文件全路径
        /// </summary>
        /// <param name="logsPath">原文件夹路径</param>
        /// <param name="suffixName">文件后缀名(记录加 "." 号)</param>
        /// <returns>文件全路径</returns>
        public static string generateFileFullName(string logsPath, string suffixName)
        {
            return String.Concat(logsPath, logsPath.EndsWith("/") ? "" : "/", DateTimeUtils.getCurrentTimeStamp(), suffixName); ;
        } // end of function combinLogPath()

        public static List<int> randomSetByFloyd(int start, int end, int count)
        {
            List<int> list = new List<int>();
            for (int i = end - count; i < end; i++) {
                int random = nextInt(start, i);
                if (list.Contains(random)) {
                    list.Add(i);
                } else {
                    list.Add(random);
                }
            }
        
            return list;
        }

        public static int nextInt(int start, int end)
        {
            return new Random().Next(Math.Abs(start - end) + 1) + Math.Min(start, end);
        }

        public static double log2(double x)
        {
            return Math.Log(x) / Math.Log(2.0);
        }
    }
}
