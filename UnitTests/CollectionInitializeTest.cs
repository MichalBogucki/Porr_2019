using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionInitializeTest
    {
        private int n = 16;
        [TestMethod]
        public void InitializeCollectionTest()
        {
            var linearSorting = new OddEvenSorting(n);
            var result = linearSorting.GetPrimalColletion();
            Equals(result.Count == n);
        }

    }
}
