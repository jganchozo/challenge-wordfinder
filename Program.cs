using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WordFinderChallenge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process currentProcess = Process.GetCurrentProcess();
            PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", currentProcess.ProcessName, true);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            float cpuUsage = cpuCounter.NextValue();

            IEnumerable<string> matrix = GetMatrix();

            IWordFinder wordFinder = new WordFinder(matrix);

            IEnumerable<string> wordstream = GetWordStream();

            IEnumerable<string> foundWords = wordFinder.Find(wordstream);

            foreach (string word in foundWords)
            {
                Console.WriteLine(word);
            }

            cpuUsage = cpuCounter.NextValue();
            stopwatch.Stop();
            TimeSpan timeElapsed = stopwatch.Elapsed;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Time elapsed: {0}", timeElapsed);
            Console.WriteLine($"CPU usage of the current process: {cpuUsage}%");
            Console.WriteLine($"CPU time of the current process: {currentProcess.TotalProcessorTime}");
            Console.ReadLine();
        }

        private static List<string> GetMatrix()
        {
            return new List<string>
            {
                "abcdc",
                "fgwio",
                "chill",
                "pqnsd",
                "uvdxy"
            };
        }

        private static List<string> GetWordStream()
        {
            return new List<string>
            {
                "cold",
                "wind",
                "snow",
                "chill"
            };
        }
    }
}
