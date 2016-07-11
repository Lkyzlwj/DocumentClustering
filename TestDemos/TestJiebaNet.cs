/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 16:25:30
** Last Modify: 2016/7/7 16:25:30
** desc：       尚未编写描述
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JiebaNet.Analyser;
using JiebaNet.Segmenter.PosSeg;
using JiebaNet.Segmenter;

namespace TestDemos
{
    class TestJiebaNet
    {
        public void execute()
        {
            var segmenter = new JiebaSegmenter();
            var segments = segmenter.Cut("这个跳满分");
            Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));
        }
    }
}
