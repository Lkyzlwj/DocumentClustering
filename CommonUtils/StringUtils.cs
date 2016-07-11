/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016‎/‎5‎/‎20
** Last Modify: 2016/5/20
** desc：       字符串工具类
** Ver.:        V0.1.0

************************************************************************/

using System;

namespace org.common.utils.str
{
    public class StringUtils
    {
        public static bool isEmpty(string text)
        {
            return text == null || text.Length == 0;
        }
    }
}
