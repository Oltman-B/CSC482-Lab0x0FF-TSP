using System;

namespace CSC482_Lab_0x0FF
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = GraphGenerator.GenerateRandomCircularGraph(10, 10);
            Console.WriteLine(graph);
        }
    }
}
