using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests;

namespace PorrConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 100;
            Console.WriteLine($"Please provide number \"n\"=");
            var consoleInput = Console.ReadLine();
            bool success = int.TryParse(consoleInput, out int consoleOutput);
            if (success)
                n = consoleOutput;

            var sorting = new OddEvenSorting(n);

            var primalColletion = sorting.GetPrimalColletion();

            var quickResult = sorting.GetPrimalQuickSortedColletion();

            var parallelResult = sorting.GetSortedParallely().ToNormalIntList();
            var sequentialResult = sorting.GetSortedSequentially().ToNormalIntList();

            var quickTime = sorting.QuickTime;
            var sequentialTime = sorting.SequentialTime;
            var parallelTime = sorting.ParallelTime;

            if (n <= 16)
                Console.WriteLine(string.Join(", ", primalColletion.ToArray()));

            Console.WriteLine($"n = {n}, quickTime = {quickTime} ms");
            Console.WriteLine($"n = {n}, sequentialTime = {sequentialTime} ms");
            Console.WriteLine($"n = {n}, parallelTime = {parallelTime} ms");

            if (n <= 16)
            {
                Console.WriteLine(string.Join(", ", sequentialResult.ToArray()));
                Console.WriteLine(string.Join(", ", parallelResult.ToArray()));
            }

            Console.WriteLine($"Press any key to exit.");
            Console.ReadLine();
        }
    }
}


