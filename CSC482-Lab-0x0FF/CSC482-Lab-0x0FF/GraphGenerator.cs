using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC482_Lab_0x0FF
{
    static class GraphGenerator
    { 
        public static Graph GenerateRandomCircularGraph(int vertexCount, int radius)
        {
            var graph = new Graph(vertexCount);
            // Randomize vertex list so that order will be different for each call
            List<int> vertexList = GenerateRandomVertexListFromZero(vertexCount);
            var xTable = new double[vertexCount];
            var yTable = new double[vertexCount];
            double stepAngle = 2 * Math.PI / vertexCount;
            for (int i = 0; i < vertexCount; i++)
            {
                // Map x and y to the correct vertex for each step around circle.
                int s = vertexList[i];
                xTable[s] = radius * Math.Sin(s * stepAngle);
                yTable[s] = radius * Math.Cos(s * stepAngle);
            }

            // Populate graph weights with distances between each vertex pair.
            // This will generate a complete Undirected Graph.
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    double dist = EuclideanDistance(xTable[i], xTable[j],yTable[i], yTable[j]);
                    graph[i, j] = dist;
                    graph[j, i] = dist;
                }
            }

            return graph;
        }

        private static double EuclideanDistance(double x1, double x2, double y1, double y2 )
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow(y1 - y2, 2));
        }

        private static List<int> GenerateRandomVertexListFromZero(int vertexCount)
        {
            // Not probably the most efficient way to handle this.
            // Fill list with vertices 1..N-1
            return Enumerable.Range(1, vertexCount - 1)
                .OrderBy(x => Guid.NewGuid())// Sort list by new GUIDs (randomization trick)
                .Prepend(0).ToList(); // Prepend 0 so that it is always the starting vertex for our testing
        }

    }
}
