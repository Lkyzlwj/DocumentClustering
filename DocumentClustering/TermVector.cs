/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 9:30:53
** Last Modify: 2016/7/7
** desc：       词元向量（因为是动态的，所以这里使用 List 存储）
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;

namespace org.machine.learning.cluster.kmeans.model
{
    public class TermVector
    {
        List<String> termVector = null;

        public TermVector()
        {
            termVector = new List<string>();
        }

        public List<String> getTermVector() {
            return termVector;
        }

        public void addTerm(String term)
        {
            if (termVector == null)
            {
                termVector = new List<String>();
            }

            termVector.Add(term);
        }

        public bool contains(String term)
        {
            if (termVector == null)
            {
                return false;
            }

            return termVector.Contains(term);
        }
    }
}
