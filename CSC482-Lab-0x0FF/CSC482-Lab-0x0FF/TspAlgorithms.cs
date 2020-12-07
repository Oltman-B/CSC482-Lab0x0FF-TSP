using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSC482_Lab_0x0FF
{
    static class TspAlgorithms
    {
        public static void RunTimeTests()
        {
            var benchmarker = new AlgorithmBenchmarker();
            benchmarker.AddAlgorithmToBenchmark(TspBruteForce, TspBruteForceDoublingCalculator);
            benchmarker.AddAlgorithmToBenchmark(TspGreedy, TspGreedyDoublingCalculator);

            benchmarker.RunTimeTests();
        }

        public static bool VerificationTests()
        {
            var graph = new EuclideanCircularGraph(10, 100);
            if (Math.Abs(TspBruteForce(graph) - graph.ShortestRouteCost) > 0.05)
            {
                return false;
            }

            return true;
        }

        public static double TspGreedy(Graph Graph)
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
                    // Next path starts from previously visited vertex
                    int nextVertex = bestRoute[^1];

                    // Skip diagonal with 0s and all visited nodes
                    if (nextVertex == j || visited[j]) continue;

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
        public static void TspGreedyDoublingCalculator(AlgStats algStats)
        {
            if (algStats.n <= 2)
            {
                algStats.ExpectedDoublingRatio = -1;
                algStats.ActualDoublingRatio = -1;
                return;
            }

            algStats.ActualDoublingRatio = algStats.TimeMicro / algStats.PrevTimeMicro;
            algStats.ExpectedDoublingRatio = algStats.n * algStats.n / (double)((algStats.n/2) * (algStats.n/2));
        }

        public static double TspBruteForce(Graph Graph)
        {
            // copy initial vertex IdList
            var bestRoute = PermuteAllRoutes(Graph.VertexIds, 1, Graph.VertexCount - 1, Graph.VertexIds, Graph);
            return Graph.CalculateRouteCost(bestRoute);
        }

        private static List<int> PermuteAllRoutes(in List<int> srce, int l, int r, List<int> bestRoute, Graph Graph)
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
                    List<int> result = PermuteAllRoutes(srceCopy, l + 1, r, bestRouteCopy, Graph);
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

        public static void TspBruteForceDoublingCalculator(AlgStats algStats)
        {
            if (algStats.n <= 2)
            {
                algStats.ExpectedDoublingRatio = -1;
                algStats.ActualDoublingRatio = -1;
                return;
            }

            algStats.ActualDoublingRatio = algStats.TimeMicro / algStats.PrevTimeMicro;
            algStats.ExpectedDoublingRatio = CalculateFactorial(algStats.n) / CalculateFactorial(algStats.n - 1);
        }

        private static double CalculateFactorial(int number)
        {
            double result = 1;
            while (number != 1)
            {
                result = result * number;
                number = number - 1;
            }
            return result;
        }

        private static void Swap(List<int> srce, int a, int b)
        {
            int temp = srce[a];
            srce[a] = srce[b];
            srce[b] = temp;
        }
    }
}
