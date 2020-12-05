using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSC482_Lab_0x0FF
{
    class Graph
    {
        private readonly double[,] _costMatrix;
        public int VertexCount { get;}

        public Graph(int vertexCount)
        {
            _costMatrix = new double[vertexCount,vertexCount];
            VertexCount = vertexCount;
        }

        public double this[int i, int j]
        {
            get => _costMatrix[i, j];
            set => _costMatrix[i, j] = value;
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
                sb.Append($"{i}  ");
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
