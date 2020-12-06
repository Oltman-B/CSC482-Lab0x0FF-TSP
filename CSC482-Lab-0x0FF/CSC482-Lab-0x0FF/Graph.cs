using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC482_Lab_0x0FF
{
    class Graph
    {
        private readonly double[,] _costMatrix;
        public int VertexCount { get; }
        public List<int> VertexIds { get; }

        public Graph(int vertexCount)
        {
            _costMatrix = new double[vertexCount, vertexCount];
            VertexCount = vertexCount;
            // Generate vertex ids 0..N
            VertexIds = Enumerable.Range(0, VertexCount).ToList();
        }

        public double this[int i, int j]
        {
            get => _costMatrix[i, j];
            set => _costMatrix[i, j] = value;
        }

        public void PrintRoute(List<int> verticesInRoute)
        {
            foreach (var v in verticesInRoute)
            {
                Console.Write($"{v}->");
            }
            Console.Write("0\n");
        }

        public double CalculateRouteCost(List<int> verticesInRoute)
        {
            double sum = 0;
            int i = 0;
            foreach (var vertex in verticesInRoute)
            {
                int j = vertex;
                sum += this[i, j];
                i = j;
            }

            return sum += this[i, 0];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0,-5:N0}", " ");
            for (int i = 0; i < _costMatrix.GetLength(0); i++)
            {
                sb.Append($"{i,-7:N0}");
            }
            sb.AppendLine();
            sb.AppendLine();

            for (int i = 0; i < _costMatrix.GetLength(0); i++)
            {
                sb.Append($"{i,-3}  ");
                for (int j = 0; j < _costMatrix.GetLength(1); j++)
                {
                    sb.Append($"{_costMatrix[i, j],-7:N1}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
