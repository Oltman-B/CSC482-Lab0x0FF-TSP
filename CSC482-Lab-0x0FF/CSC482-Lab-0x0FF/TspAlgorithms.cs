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

        public double TspBruteForce()
        {
            // copy initial vertex IdList
            List<int> bestRoute = Graph.VertexIds.ToList();
            bestRoute = PermuteAllRoutes(Graph.VertexIds, 1, Graph.VertexCount - 1, bestRoute);
            return Graph.CalculateRouteCost(bestRoute);
        }

        private List<int> PermuteAllRoutes(List<int> srce, int l, int r, List<int> bestRoute)
        {
            if (l == r)
            {
                //TODO: calculating bestRouteCost every time is not efficient, replace this
                double bestRouteCost = Graph.CalculateRouteCost(bestRoute);
                double thisRouteCost = Graph.CalculateRouteCost(srce);
                if (thisRouteCost < bestRouteCost) bestRoute = srce;
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    Swap(srce, l, i);
                    PermuteAllRoutes(srce, l + 1, r, bestRoute);
                    Swap(srce, l, i);
                }
            }

            return bestRoute;
        }

        private void Swap(List<int> srce, int a, int b)
        {
            int temp = srce[a];
            srce[a] = srce[b];
            srce[b] = temp;
        }
    }
}
