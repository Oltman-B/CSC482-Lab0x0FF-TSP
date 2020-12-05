using System;

namespace CSC482_Lab_0x0FF
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new EuclideanCircularGraph(10, 100);
            TspAlgorithms tspSandbox = new TspAlgorithms(graph);
            Console.WriteLine($"Brute Force Result = {tspSandbox.TspBruteForce()}");
            Console.WriteLine($"Expected Result = {graph.ShortestRouteCost}");
            //Console.WriteLine(graph);
        }
    }
}
