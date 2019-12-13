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
            int n = 20000;
            
            var linearSorting = new OddEvenSorting(n);
            var preResult = linearSorting.GetPrimalColletion();
            var parallelResult = linearSorting.GetSortedParallely();
            var seqResult = linearSorting.GetSortedSequentially();
            var parallelTime = linearSorting.parallelTime;
            var sequentialTime = linearSorting.sequentialTime;
            
            if (n <= 16)
                Console.WriteLine(string.Join(", ", preResult.ToArray()));
            //todo delete me
            Console.WriteLine($"n = {n}, sequentialTime = {sequentialTime} ms");
            Console.WriteLine($"n = {n}, parallelTime = {parallelTime} ms");
            
            if (n <= 16)
                Console.WriteLine(string.Join(", ", seqResult.ToArray()));
            
            Console.WriteLine("Hello World!");
            Console.ReadKey();
            
        }
    }
}
