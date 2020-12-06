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
            var bestRoute = new List<int>();

            for (int i = 0; i < Graph.VertexCount; i++)
            {
                double minWeight = double.PositiveInfinity;
                int vertexCandidate = -1;
                visited[i] = true;
                for (int j = 0; j < Graph.VertexCount; j++)
                {
                    if (j == i || visited[j]) continue;
                    if (Graph[i, j] < minWeight)
                    {
                        minWeight = Graph[i, j];
                        vertexCandidate = j;
                    }
                }

                if (vertexCandidate > -1)
                {
                    bestRoute.Add(vertexCandidate);
                }
            }

            foreach (var i in bestRoute)
            {
                Console.Write($"{i}, ");
            }
            return Graph.CalculateRouteCost(bestRoute);

        }
        public double TspBruteForce()
        {
            // copy initial vertex IdList
            var bestRoute = PermuteAllRoutes(Graph.VertexIds.ToList(), 1, Graph.VertexCount - 1, Graph.VertexIds.ToList());
            foreach (var i in bestRoute)
            {
                Console.Write($"{i}, ");
            }
            return Graph.CalculateRouteCost(bestRoute);
        }

        private List<int> PermuteAllRoutes(List<int> srce, int l, int r, List<int> bestRoute)
        {
            // Make copy of lists so that they can be modified in each recursive call.
            List<int> source = srce.ToList();
            List<int> shortRoute = bestRoute.ToList();

            double bestRouteCost = Graph.CalculateRouteCost(shortRoute);
            if (l == r)
            {
                double thisRouteCost = Graph.CalculateRouteCost(source);
                if (thisRouteCost < bestRouteCost) shortRoute = source;
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    Swap(source, l, i);
                    List<int> result = PermuteAllRoutes(source, l + 1, r, shortRoute);
                    double resultCost = Graph.CalculateRouteCost(result);
                    if (resultCost < bestRouteCost)
                    {
                        shortRoute = result;
                        bestRouteCost = Graph.CalculateRouteCost(shortRoute);
                    }
                    Swap(source, l, i);
                }
            }

            var tempCost = Graph.CalculateRouteCost(shortRoute);
            return shortRoute;
        }

        private void Swap(List<int> srce, int a, int b)
        {
            int temp = srce[a];
            srce[a] = srce[b];
            srce[b] = temp;
        }
    }
}
