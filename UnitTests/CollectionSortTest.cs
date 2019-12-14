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
            Console.WriteLine($"n = {n}, parallelTime = {parallelTime} ms");
            Console.WriteLine(string.Join(", ", pararellResult.ToArray()));

            if (n <= 16)
            {
                Console.WriteLine(string.Join(", ", sequentialResult.ToArray()));
                Console.WriteLine(string.Join(", ", pararellResult.ToArray()));
            }
            
            CollectionAssert.AreEqual(sequentialResult, quickResult);
            CollectionAssert.AreEqual(sequentialResult, pararellResult);
        }
    }
}
