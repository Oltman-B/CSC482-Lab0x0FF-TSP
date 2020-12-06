using System;
using System.Collections.Generic;
using System.Linq;

namespace CSC482_Lab_0x0FF
{
    class EuclideanCircularGraph : Graph
    {
        public List<int> ShortestRoute { get; private set; }
        public double ShortestRouteCost { get; }

        public EuclideanCircularGraph(int vertexCount, int radius) : base(vertexCount)
        {
            GenerateRandomCircularGraph(vertexCount, radius);
            ShortestRouteCost = vertexCount * this[ShortestRoute[0], ShortestRoute[1]];
        }

        private void GenerateRandomCircularGraph(int vertexCount, int radius)
        {
            // Randomize vertex list so that order will be different for each call
            List<int> vertexList = RandomizeVertexListFromZero(base.VertexIds);
            var xTable = new double[vertexCount];
            var yTable = new double[vertexCount];
            double stepAngle = 2 * Math.PI / vertexCount;
            for (int i = 0; i < vertexCount; i++)
            {
                // Map x and y to the correct vertex for each step around circle.
                xTable[i] = radius * Math.Sin(i * stepAngle);
                yTable[i] = radius * Math.Cos(i * stepAngle);
            }

            // Populate graph weights with distances between each vertex pair.
            // This will generate a complete Undirected Graph.
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    int a = vertexList[i];
                    int b = vertexList[j];
                    double dist = EuclideanDistance(xTable[i], xTable[j], yTable[i], yTable[j]);
                    //Calculate the distance as if vertexList was in order, but then map to graph correctly.
                    this[a, b] = dist;
                    this[b, a] = dist;
                }
            }

            ShortestRoute = vertexList;
        }

        private static double EuclideanDistance(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow(y1 - y2, 2));
        }

        private static List<int> RandomizeVertexListFromZero(IEnumerable<int> vertices)
        {
            // Not probably the most efficient way to handle this.
            // Sort list by new GUIDs (randomization trick) skip 0 because it has to be prepended.
            vertices = vertices.Skip(1).OrderBy(x => Guid.NewGuid());
            return vertices.Prepend(0).ToList();
        }
    }
}
