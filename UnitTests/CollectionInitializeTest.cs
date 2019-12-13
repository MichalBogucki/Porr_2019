using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CollectionInitializeTest
    {
        private int n = 12;
        [TestMethod]
        public void InitializeCollectionTest()
        {
            
            var linearSorting = new OddEvenSorting(n);


            //a = source array
            //1 = start index in source array
            //    b = destination array
            //0 = start index in destination array
            //3 = elements to copy

            var aaa = new List<RefInt>()
            {
                new RefInt(1),new RefInt(2),new RefInt(3),new RefInt(4)
            };


            var ddd = new List<RefInt>();
            foreach (var item in aaa)
            {
                ddd.Add(new RefInt(item.refint));
            }
            var result = linearSorting.GetPrimalColletion();

            var refResult = linearSorting.GetSortedRefParallely();
            //var sourceArray = result;
            var startIndexInSourceArray = 1;
            var startIndexInDestionationArray = 0;
            var elementsToCopy = 2;
            //
            //Array.Copy(sourceArray.ToArray(), startIndexInSourceArray, destinationArray, startIndexInDestionationArray, elementsToCopy);
            var bbb = aaa;
            bbb[0].refint = 77;
            var ccc  = aaa.GetRange(1, aaa.Count-2);
            
            Equals(aaa[0].refint == 77);

            var firstBatch = refResult.refIntList.GetRange(1, refResult.refIntList.Count - 2);
            var secondBatch = refResult.refIntList.GetRange(0, refResult.refIntList.Count - 1);


            var oddBatches = new List<List<RefInt>>();
            oddBatches.Add(refResult.refIntList.GetRange(0, n / 4));
            oddBatches.Add(refResult.refIntList.GetRange(n / 4, n / 4));
            oddBatches.Add(refResult.refIntList.GetRange(n / 2, n / 4));
            oddBatches.Add(refResult.refIntList.GetRange(n * 3 / 4, n / 4));

            Equals(result.Count == n);
        }

    }
}
