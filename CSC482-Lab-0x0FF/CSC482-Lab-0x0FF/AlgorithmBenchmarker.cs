﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSC482_Lab_0x0FF
{
    class AlgStats
    {
        public int n = 0;
        public Dictionary<int, double> PrevTimesTable = new Dictionary<int, double>();
        public double PrevTimeMicro = 0;
        public double TimeMicro = 0;
        public double ExpectedDoublingRatio = 0;
        public ValueType AlgResult;
        public ValueType CorrectnessRatio;
        public double ActualDoublingRatio = 0;
    }

    class AlgorithmBenchmarker
    {
        private const double MaxSecondsPerAlgorithm = 15;
        private const double MaxMicroSecondsPerAlg = MaxSecondsPerAlgorithm * 1000000;

        private const double MaxSecondsPerIteration = 3;
        private const double MaxMicroSecondsPerIteration = MaxSecondsPerIteration * 1000000;
        

        private const int NMin = 2;
        private const int NMax = 32768;

        private readonly Random _rand = new Random();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        // To use benchmarker, simply define the delegate with the signature of your algorithm to test and also the data
        // source it will use.
        internal delegate double Algorithm(Graph graph);
        // The algorithms are responsible for defining their own doubling ratio calculator
        internal delegate void DoublingCalculator(AlgStats stats);

        // This concrete AlgorithmBenchmark implementation will operate on a list of integers
        private List<Algorithm> _algorithms = new List<Algorithm>();
        private List<DoublingCalculator> _doublingCalculators = new List<DoublingCalculator>();

        // Called from within the scope of your algorithms instantiation, simply pass the algorithm function name
        // and the doublingcalculator function name as parameters. Call RunTimeTests to run each algorithm added
        // and display statistics based on doubling calculator.
        public void AddAlgorithmToBenchmark(Algorithm algorithm, DoublingCalculator doublingCalc)
        {
            _algorithms.Add(algorithm);
            _doublingCalculators.Add(doublingCalc);
        }

        public void RunTimeTests()
        {
            Debug.Assert(_algorithms.Count == _doublingCalculators.Count);

            for (int i = 0; i < _algorithms.Count; i++)
            {
                AlgorithmTestRuntime(_algorithms[i], _doublingCalculators[i]);
            }
        }
        private void AlgorithmTestRuntime(Algorithm algorithm, DoublingCalculator doublingCalc)
        {
            PrintHeader(algorithm);

            var currentStats = new AlgStats();

            for (var n = NMin; n <= NMax; n++)
            {
                currentStats.n = n;
                if (currentStats.TimeMicro > MaxMicroSecondsPerAlg)
                {
                    PrintAlgorithmTerminationMessage(algorithm);
                    break;
                }

                PrintIndexColumn(currentStats.n);

                int testCount = 1;
                int maxTest = 1000000;
                long tickCounter = 0;
                while (testCount <= maxTest && TicksToMicroseconds(tickCounter) < MaxMicroSecondsPerIteration)
                {
                    Graph graph = new EuclideanCircularGraph(n, 100);
                    _stopwatch.Restart();
                    currentStats.AlgResult = algorithm(graph);
                    _stopwatch.Stop();
                    // HACK (this should be handled better)
                    currentStats.CorrectnessRatio =
                        ((EuclideanCircularGraph)graph).ShortestRouteCost / (double)currentStats.AlgResult;
                    tickCounter += _stopwatch.ElapsedTicks;
                    testCount++;
                }

                double averageTimeMicro = TicksToMicroseconds(tickCounter) / testCount;
                

                currentStats.PrevTimeMicro = currentStats.TimeMicro;
                currentStats.TimeMicro = averageTimeMicro;
                // Need to keep a dictionary of previous times for doubling calculation on this alg.
                currentStats.PrevTimesTable.TryAdd(currentStats.n, averageTimeMicro);

                doublingCalc(currentStats);

                PrintData(currentStats);

                // New Row
                Console.WriteLine();
            }
        }

        // Should be abstracted out so that column names etc, can be passed and this function doesn't have
        // to be modified in source code.
        private void PrintHeader(Algorithm algorithm)
        {
            Console.WriteLine($"Starting run-time tests for {algorithm.Method.Name}...\n");
            Console.WriteLine(
                " \t\t\t           |    |            Plus One Ratios     |    |    Algorithm|   |             Result|");
            Console.WriteLine(
                "X\t\t\t       Time|    |         Actual|        Expected|    |       Result|   |  Correctness Ratio|");
        }

        private void PrintAlgorithmTerminationMessage(Algorithm algorithm)
        {
            Console.WriteLine($"{algorithm.Method.Name} exceeded allotted time, terminating...\n");
        }

        private void PrintIndexColumn(int n)
        {
            Console.Write($"{n,-15}");
        }

        private void PrintData(AlgStats stats)
        {
            var actualDoubleFormatted = stats.ActualDoublingRatio < 0
                ? "na".PadLeft(20)
                : stats.ActualDoublingRatio.ToString("F2").PadLeft(20);
            var expectDoubleFormatted = stats.ExpectedDoublingRatio < 0
                ? "na".PadLeft(16)
                : stats.ExpectedDoublingRatio.ToString("F2").PadLeft(16);

            Console.Write(
                $"{stats.TimeMicro,20:F2} {actualDoubleFormatted} {expectDoubleFormatted}" +
                $"{stats.AlgResult,19:N0} {stats.CorrectnessRatio,23:P1}");
        }

        private static double TicksToMicroseconds(long ticks)
        {
            return (double) ticks / Stopwatch.Frequency * 1000000;
        }
    }
}