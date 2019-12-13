using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionSortTest
    {
        private int n = 10000;

        [TestMethod]
        public void LinearSortCollectionTest()
        {
            var linearSorting = new OddEvenSorting(n);
            var preResult = linearSorting.GetPrimalColletion();
            var seqResult = linearSorting.GetSortedSequentially();
            var parallelResult = linearSorting.GetSortedParallely();
            
            var parallelTime = linearSorting.parallelTime;
            var sequentialTime = linearSorting.sequentialTime;

            if (n <= 16)
                Console.WriteLine(string.Join(", ", preResult.ToArray()));
            //todo delete me
            Console.WriteLine($"n = {n}, sequentialTime = {sequentialTime} ms");
            Console.WriteLine($"n = {n}, parallelTime = {parallelTime} ms");

            if (n <= 16)
                Console.WriteLine(string.Join(", ", seqResult.ToArray()));

            Equals(seqResult, preResult);
            Equals(preResult.Count == n);
        }
    }
}
