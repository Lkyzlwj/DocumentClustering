/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 16:28:21
** Last Modify: 2016/7/7
** desc：       尚未编写描述
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using org.machine.learning.tfidf;
using org.machine.learning.cluster.kmeans.utils;
using org.machine.learning.cluster.kmeans;
using org.machine.learning.cluster.kmeans.model;
using org.common.utils.file;

namespace TestDemos
{
    class TestKmeans
    {
        public void execute()
        {
            List<string> documents = FileUtils.read(String.Concat(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\input.txt"));

            Kmeans kmeans = new Kmeans();
            List<CentroidVector> centroids = kmeans.start(documents, 3);

            foreach (CentroidVector centroid in centroids)
            {
                foreach (DocumentVector dv in centroid.getVectorCluster())
                {
                    Console.WriteLine(dv.getLabel());
                }
                Console.WriteLine("\n-------------{0}-------------------\n", centroid.getVectorCluster().Count);
                Thread.Sleep(100);
            }
        }
    }
}
