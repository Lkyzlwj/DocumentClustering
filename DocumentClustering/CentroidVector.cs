/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 14:02:36
** Last Modify: 2016/7/7
** desc：       质心向量
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.machine.learning.cluster.kmeans.model
{
    public class CentroidVector : DocumentVector
    {
        private List<DocumentVector> vectorCluster = null;

        public CentroidVector()
        {
            vectorCluster = new List<DocumentVector>();
        }

        public List<DocumentVector> getVectorCluster()
        {
            return vectorCluster;
        }

        public void addVector(DocumentVector vector)
        {
            if (vectorCluster == null)
            {
                vectorCluster = new List<DocumentVector>();
            }

            vectorCluster.Add(vector);
        }

        public void clearCluster()
        {
            if (vectorCluster != null)
            {
                vectorCluster.Clear();
            }
        }

        public void updateDimensionValue(int dimension, double value)
        {
            weightVector[dimension] = value;
        }

        public override string ToString()
        {
            return String.Join(", ", weightVector);
        }
    }
}
