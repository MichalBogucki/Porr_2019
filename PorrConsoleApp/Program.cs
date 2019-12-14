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

            var linearSorting = new OddEvenSorting(n);

            var preResult = linearSorting.GetPrimalColletion();

            var quickResult = linearSorting.GetPrimalQuickSorktedColletion();
            var sequentialResult = linearSorting.GetSortedSequentially().ToNormalIntList();
            var pararellResult = linearSorting.GetSortedParallely().ToNormalIntList();

            var quickTime = linearSorting.QuickTime;
            var sequentialTime = linearSorting.SequentialTime;

            var parallelTime = linearSorting.ParallelTime;


            if (n <= 16)
                Console.WriteLine(string.Join(", ", preResult.ToArray()));

            Console.WriteLine($"n = {n}, quickTime = {quickTime} ms");
            Console.WriteLine($"n = {n}, sequentialTime = {sequentialTime} ms");
            Console.WriteLine($"n = {n}, referenceTime = {parallelTime} ms");
            Console.WriteLine(string.Join(", ", pararellResult.ToArray()));

            if (n <= 16)
            {
                Console.WriteLine(string.Join(", ", sequentialResult.ToArray()));
                Console.WriteLine(string.Join(", ", pararellResult.ToArray()));
            }
        }
    }
}


