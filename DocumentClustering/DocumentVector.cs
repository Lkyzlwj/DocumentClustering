/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 9:39:23
** Last Modify: 2016/7/7
** desc：       每个 Document 所代表的文档向量
 *              DocumentVector = {weight1, weight2, weight3 …… weightN}
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.machine.learning.cluster.kmeans.model
{
    public class DocumentVector
    {
        private string label = string.Empty;
        protected List<double> weightVector = null;

        public DocumentVector()
        {
            weightVector = new List<double>();
        }

        public List<double> getWeightVector()
        {
            return weightVector;
        }

        public void addWeight(double weight)
        {
            if (weightVector == null)
            {
                weightVector = new List<double>();
            }

            weightVector.Add(weight);
        }

        public void setLabel(string label)
        {
            this.label = label;
        }

        public string getLabel()
        {
            return label;
        }

        public override string ToString()
        {
            return String.Join(", ", weightVector);
        }
    }
}
