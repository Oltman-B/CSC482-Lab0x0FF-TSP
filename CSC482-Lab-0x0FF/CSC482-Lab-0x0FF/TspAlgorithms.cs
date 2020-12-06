using System;
using System.Collections.Generic;
using System.Linq;

namespace CSC482_Lab_0x0FF
{
    class TspAlgorithms
    {
        public TspAlgorithms(Graph graph)
        {
            Graph = graph;
        }

        public Graph Graph { get; set; }

        public double TspGreedy()
        {
            var visited = new bool[Graph.VertexCount];
            //Always starting from 0
            var bestRoute = new List<int>() { 0 };

            // Iterate through entire graph structure
            for (int i = 0; i < Graph.VertexCount; i++)
            {
                double minWeight = double.PositiveInfinity;
                int vertexCandidate = -1;
                for (int j = 1; j < Graph.VertexCount; j++)
                {
                    // Skip diagonal with 0s and all visited nodes
                    if (j == i || visited[j]) continue;

                    // Next path starts from previously visited vertex
                    int nextVertex = bestRoute[^1];

                    // If next path in attempt is less than min weight, update min weight
                    // and save vertex as potential solution.
                    if (Graph[nextVertex, j] < minWeight)
                    {
                        minWeight = Graph[nextVertex, j];
                        vertexCandidate = j;
                    }
                }

                // After checking all paths at current vertex
                // add candidate if a path exists.
                if (vertexCandidate > -1)
                {
                    bestRoute.Add(vertexCandidate);
                    // Mark added node as visited.
                    visited[vertexCandidate] = true;
                }
            }
            return Graph.CalculateRouteCost(bestRoute);
        }

        public double TspBruteForce()
        {
            // copy initial vertex IdList
            var bestRoute = PermuteAllRoutes(Graph.VertexIds, 1, Graph.VertexCount - 1, Graph.VertexIds);
            return Graph.CalculateRouteCost(bestRoute);
        }

        private List<int> PermuteAllRoutes(in List<int> srce, int l, int r, List<int> bestRoute)
        {
            // Make copy of lists so that they can be modified in each recursive call.
            List<int> srceCopy = srce.ToList();
            List<int> bestRouteCopy = bestRoute.ToList();

            double bestRouteCost = Graph.CalculateRouteCost(bestRouteCopy);
            if (l == r)
            {
                double thisRouteCost = Graph.CalculateRouteCost(srceCopy);
                if (thisRouteCost < bestRouteCost) bestRouteCopy = srceCopy;
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    Swap(srceCopy, l, i);
                    List<int> result = PermuteAllRoutes(srceCopy, l + 1, r, bestRouteCopy);
                    double resultCost = Graph.CalculateRouteCost(result);
                    if (resultCost < bestRouteCost)
                    {
                        bestRouteCopy = result;
                        bestRouteCost = Graph.CalculateRouteCost(bestRouteCopy);
                    }
                    Swap(srceCopy, l, i);
                }
            }

            var tempCost = Graph.CalculateRouteCost(bestRouteCopy);
            return bestRouteCopy;
        }

        private void Swap(List<int> srce, int a, int b)
        {
            int temp = srce[a];
            srce[a] = srce[b];
            srce[b] = temp;
        }
    }
}
