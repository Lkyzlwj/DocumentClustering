/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 9:23:45
** Last Modify: 2016/7/11
** desc：       K-means 的核心算法类
** Ver.:        V0.1.2

************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using org.common.utils;
using org.machine.learning.cluster.kmeans.model;
using org.machine.learning.cluster.kmeans.utils;
using org.common.utils.file;

namespace org.machine.learning.cluster.kmeans
{
    public class Kmeans
    {
        int k = 0;
        int iterateTimes = 0;
        double deviationInterval = 1.0;
        double lastDeviation = 0.0;

        public List<CentroidVector> start(List<String> documents, int k)
        {
            this.k = k;
            List<DocumentVector> vectors = KmeansUtils.transformDocuments2Vectors(documents);
            List<CentroidVector> centroids = randomCentroidVectorList3(vectors);

            kmeansCore(vectors, centroids);

            return centroids;
        }

        private void kmeansCore(List<DocumentVector> vectors, List<CentroidVector> centroids)
        {
            Console.WriteLine("K-means++ 文本聚类，正在进行第 {0} 次迭代...", iterateTimes++);
            clearClusters(centroids);
            fillClusters(vectors, centroids);

            double currentDeviation = getPointDistanceSquareSum(centroids);

            if (!isDone(centroids, currentDeviation))
            {
                // move
                moveCentroid(centroids);

                lastDeviation = currentDeviation;

                // kmeans
                kmeansCore(vectors, centroids);
            }
        }

        private void clearClusters(List<CentroidVector> centroids)
        {
            foreach (CentroidVector centroidVector in centroids)
            {
                centroidVector.clearCluster();
            }
        }

        private void fillClusters(List<DocumentVector> vectors, List<CentroidVector> centroids)
        {
            foreach (DocumentVector documentVector in vectors)
            {
                double minDistance = Double.MaxValue;
                CentroidVector minDistanceCentroid = null; // TODO
                foreach (CentroidVector centroidVector in centroids)
                {
                    double currentDistance = cosSimilarity(documentVector, centroidVector);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        minDistanceCentroid = centroidVector;
                    }
                }
                minDistanceCentroid.addVector(documentVector);
            }
        }

        private bool isDone(List<CentroidVector> centroids, double currentDeviation)
        {
            // Console.WriteLine("误差值: {0}", Math.Abs(currentDeviation - lastDeviation));
            if (Math.Abs(currentDeviation - lastDeviation) > deviationInterval)
            {
                return false;
            }

            return true;
        }

        private bool isDone2(List<CentroidVector> centroids, double currentDeviation)
        {
            foreach (CentroidVector centroid in centroids)
            {
                List<DocumentVector> subCluster = centroid.getVectorCluster();
                foreach (DocumentVector documentVector in subCluster)
                {
                    if (!centroid.Equals(getClosestCentroid(centroids, documentVector)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private CentroidVector getClosestCentroid(List<CentroidVector> centroids, DocumentVector documentVector)
        {
            double minDistance = Double.MaxValue;
            CentroidVector minCentroidVector = null;
            foreach (CentroidVector centroid in centroids)
            {
                double currentDistance = cosSimilarity(documentVector, centroid);
                if (minDistance > currentDistance)
                {
                    minDistance = currentDistance;
                    minCentroidVector = centroid;
                }
            }

            return minCentroidVector;
        }

        private double getPointDistanceSquareSum(List<CentroidVector> centroids) {
            double distanceSum = 0.0;
            foreach (CentroidVector centroid in centroids) {
                List<DocumentVector> subCluster = centroid.getVectorCluster();
                foreach (DocumentVector centroidMember in subCluster) {
                    distanceSum += cosSimilarity(centroidMember, centroid);
                }
            }
        
            return distanceSum;
        }

        private List<CentroidVector> randomCentroidVectorList(List<DocumentVector> vectors)
        {
            if(vectors.Count < k)
            {
                Console.WriteLine("Sorry, no more vector to random.");
                return null;
            }

            List<CentroidVector> centroids = new List<CentroidVector>();
            List<int> indexs = CommonUtils.randomSetByFloyd(0, vectors.Count, k);
            foreach (int index in indexs)
            {
                DocumentVector documentVector = vectors[index];
                CentroidVector centroid = new CentroidVector();
                foreach (double weight in documentVector.getWeightVector())
                {
                    centroid.addWeight(weight);
                }
                centroids.Add(centroid);
            }

            return centroids;
        }

        private List<CentroidVector> randomCentroidVectorList2(List<DocumentVector> vectors)
        {
            List<CentroidVector> centroids = new List<CentroidVector>();
            Random randomSeed = new Random(1);
            for (int index = 0; index < k; index++)
            {
                int randomIndex = randomSeed.Next(vectors.Count);
                DocumentVector documentVector = vectors[randomIndex];
                CentroidVector centroid = new CentroidVector();
                foreach (double weight in documentVector.getWeightVector())
                {
                    centroid.addWeight(weight);
                }
                centroids.Add(centroid);
            }

            return centroids;
        }

        /// <summary>
        /// K-means++
        /// </summary>
        private List<CentroidVector> randomCentroidVectorList3(List<DocumentVector> vectors)
        {
            List<CentroidVector> centroids = new List<CentroidVector>();
            int randomIndex = new Random(1).Next(vectors.Count);

            for (int index = 0; index < k; index++)
            {
                DocumentVector documentVector = null;
                if (index == 0)
                {
                    documentVector = vectors[randomIndex];
                }
                else
                {
                    documentVector = nextCentroidVector(vectors, centroids);
                }

                centroids.Add(transform2CentroidVector(documentVector));
            }

            return centroids;
        }

        private DocumentVector nextCentroidVector (List<DocumentVector> vectors, List<CentroidVector> centroids)
        {
            double maxClosedDistance = 0.0;
            DocumentVector maxClosedVector = null;
            foreach (DocumentVector vector in vectors)
            {
                double minDistance = Double.MaxValue;
                foreach (CentroidVector centroid in centroids)
                {
                    double currentDistance = cosSimilarity(vector, centroid);
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                    }
                }

                if (maxClosedDistance < minDistance)
                {
                    maxClosedDistance = minDistance;
                    maxClosedVector = vector;
                }
            }

            return maxClosedVector;
        }

        private CentroidVector transform2CentroidVector(DocumentVector documentVector)
        {
            CentroidVector centroid = new CentroidVector();
            foreach (double weight in documentVector.getWeightVector())
            {
                centroid.addWeight(weight);
            }

            return centroid;
        }

        private double cosSimilarity(DocumentVector vector1, DocumentVector vector2)
        {
            return 1 - cos(vector1, vector2);
        }

        private double cos(DocumentVector vector1, DocumentVector vector2)
        {
            int dimensions = vector1.getWeightVector().Count;
            double molecular = 0.0;
            double denominator = 0.0;
            double denominatorLeft = 0.0;
            double denominatorRight = 0.0;
            for (int index = 0; index < dimensions; index++)
            {
                molecular += (vector1.getWeightVector()[index] * vector2.getWeightVector()[index]);
                denominatorLeft += (vector1.getWeightVector()[index] * vector1.getWeightVector()[index]);
                denominatorRight += (vector2.getWeightVector()[index] * vector2.getWeightVector()[index]);
            }

            denominator = Math.Sqrt(denominatorLeft) * Math.Sqrt(denominatorRight);

            return molecular / denominator;
        }

        private double distance(DocumentVector vector1, DocumentVector vector2)
        {
            double distance = 0.0;
            int dimensions = vector1.getWeightVector().Count;
            for (int index = 0; index < dimensions; index++)
            {
                distance += ((vector1.getWeightVector()[index] - vector2.getWeightVector()[index]) * (vector1.getWeightVector()[index] - vector2.getWeightVector()[index]));
            }

            return Math.Sqrt(distance);
        }

        private void moveCentroid(List<CentroidVector> centroids)
        {
            foreach (CentroidVector centroidVector in centroids)
            {
                List<DocumentVector> cluster = centroidVector.getVectorCluster();
                int dimensions = centroidVector.getWeightVector().Count;
                for (int dimension = 0; dimension < dimensions; dimension++)
                {
                    foreach (DocumentVector documentVector in cluster)
                    {
                        centroidVector.updateDimensionValue(dimension, centroidVector.getWeightVector()[dimension] + documentVector.getWeightVector()[dimension]);
                    }
                }

                for (int dimension = 0; dimension < dimensions; dimension++)
                {
                    centroidVector.updateDimensionValue(dimension, centroidVector.getWeightVector()[dimension] / cluster.Count);
                }
            }
        }
    }
}
