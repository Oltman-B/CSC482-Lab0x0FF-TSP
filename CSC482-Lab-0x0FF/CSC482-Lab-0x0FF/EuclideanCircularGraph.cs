﻿using System;
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
            ShortestRouteCost = vertexCount * this[0, 1];
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
                    double dist = EuclideanDistance(xTable[i], xTable[j], yTable[i], yTable[j]);
                    this[i, j] = dist;
                    this[j, i] = dist;
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