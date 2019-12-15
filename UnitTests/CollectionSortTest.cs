using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionSortTest
    {
        [DataTestMethod]
        [DataRow(16)]
        [DataRow(32)]
        [DataRow(64)]
        [DataRow(128)]
        [DataRow(256)]
        [DataRow(512)]
        [DataRow(1024)]
        [DataRow(2048)]
        [DataRow(4096)]
        [DataRow(8192)]
        [DataRow(16384)]
        [DataRow(24576)]
        [DataRow(32728)]
        [DataRow(49152)]
        [DataRow(65536)]
        public void LinearSortCollectionTest(int n)
        {
            var sorting = new OddEvenSorting(n);
            
            var primalColletion = sorting.GetPrimalColletion();

            var quickResult = sorting.GetPrimalQuickSortedColletion();
            var sequentialResult = sorting.GetSortedSequentially().ToNormalIntList();
            var parallelResult = sorting.GetSortedParallely().ToNormalIntList();

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
            
            CollectionAssert.AreEqual(sequentialResult, quickResult);
            CollectionAssert.AreEqual(sequentialResult, parallelResult);
        }

        [DataTestMethod]
        [DataRow(10)]
        public void LinearSortCllectionTest2(int n)
        {
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

            CollectionAssert.AreEqual(sequentialResult, quickResult);
            CollectionAssert.AreEqual(sequentialResult, parallelResult);
        }
    }
}
