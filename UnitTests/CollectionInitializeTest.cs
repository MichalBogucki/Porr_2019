using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionInitializeTest
    {
        [DataRow(16)]
        public void InitializeCollectionTest(int n)
        {
            var linearSorting = new OddEvenSorting(n);
            var result = linearSorting.GetPrimalColletion();
            Equals(result.Count == n);
        }

    }
}
