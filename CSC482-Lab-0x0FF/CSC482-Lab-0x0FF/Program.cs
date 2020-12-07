using System;
using System.Collections.Generic;

namespace CSC482_Lab_0x0FF
{
    class Program
    {
        static void Main(string[] args)
        {
            if (TspAlgorithms.VerificationTests())
            {
                Console.WriteLine("Graph generation correct, moving to run time tests.");
                TspAlgorithms.RunTimeTests();
            }
            else
            {
                Console.WriteLine("There was a problem with your graph.");
            }
        }
    }
}
