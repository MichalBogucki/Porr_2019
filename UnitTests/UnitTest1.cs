using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private int n = 100000;
        [TestMethod]
        public void InitializeCollectionTest()
        {
            
            var linearSorting = new OddEvenSorting(n);
            var result = linearSorting.GetPrimalColletion();
            Equals(result.Count == n);
        }

        [TestMethod]
        public void LinearSortCollectionTest()
        {
            var linearSorting = new OddEvenSorting(n);
            var preResult = linearSorting.GetPrimalColletion();
            var seqResult = linearSorting.GetSortedSequentially();
            var parallelResult = linearSorting.GetSortedParallely();
            var sequentialTime = linearSorting.sequentialTime;
            var parallelTime = linearSorting.parallelTime;

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
