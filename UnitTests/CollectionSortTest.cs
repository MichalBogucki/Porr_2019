using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionSortTest
    {
        private int n = 20000;

        [TestMethod]
        public void LinearSortCollectionTest()
        {
            var linearSorting = new OddEvenSorting(n);
            linearSorting.DoitQuick();
            var preResult = linearSorting.GetPrimalColletion();
            
            var parallelResult = linearSorting.GetSortedParallely();

            var refResult = linearSorting.GetSortedRefParallely();
            var refAsNormalResult = refResult.ToNormalIntList();

            var seqResult = linearSorting.GetSortedSequentially();

            var parallelTime = linearSorting.parallelTime;
            var sequentialTime = linearSorting.sequentialTime;
            var referenceTime = linearSorting.referentialTime;
            var quickTime = linearSorting.quickTime;

            if (n <= 16)
                Console.WriteLine(string.Join(", ", preResult.ToArray()));
            
            Console.WriteLine($"n = {n}, sequentialTime = {sequentialTime} ms");
            Console.WriteLine($"n = {n}, parallelTime = {parallelTime} ms");
            Console.WriteLine($"n = {n}, referenceTime = {referenceTime} ms");
            Console.WriteLine($"n = {n}, quickTime = {quickTime} ms");

            if (n <= 16)
            {
                Console.WriteLine(string.Join(", ", seqResult.ToArray()));
                //Console.WriteLine(string.Join(", ", parallelResult.ToArray()));
                Console.WriteLine(string.Join(", ", refAsNormalResult.ToArray()));
            }

            Equals(seqResult, preResult);
            Equals(seqResult, refAsNormalResult);
            Equals(preResult.Count == n);
        }
    }
}
