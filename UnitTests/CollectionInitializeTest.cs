using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionInitializeTest
    {
        private int n = 40000;
        [TestMethod]
        public void InitializeCollectionTest()
        {
            
            var linearSorting = new OddEvenSorting(n);
            //var batch = linearSorting.
            var result = linearSorting.GetPrimalColletion();
            Equals(result.Count == n);
        }

    }
}
