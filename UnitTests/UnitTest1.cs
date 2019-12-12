using System;
using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private int n = 16;
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
            var postResult = linearSorting.GetSortedSequentially();
            var executionTime = linearSorting.sequentialTime;

            if (n <= 16)
                Console.WriteLine(string.Join(", ", preResult.ToArray()));

            Console.WriteLine($"execution time = {executionTime} ms");

            if (n <= 16)
                Console.WriteLine(string.Join(", ", postResult.ToArray()));

            Equals(preResult.Count == n);
            Equals(postResult.Count == n);
        }
    }
}
