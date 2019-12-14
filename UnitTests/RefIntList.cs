using System.Collections.Generic;

namespace UnitTests
{
    public class  RefIntList{
        public List<RefInt> refIntList { get; set; } = new List<RefInt>();
        
        public RefIntList(RefIntList refIntListToCopy)
        {
            foreach (var item in refIntListToCopy.refIntList)
            {
                refIntList.Add(new RefInt(item.refint));
            }
        }

        public RefIntList(List<RefInt> refIntListToCopy)
        {
            foreach (var item in refIntListToCopy)
            {
                refIntList.Add(new RefInt(item.refint));
            }
        }

        public RefIntList(List<int> intListToCopy)
        {
            foreach (var item in intListToCopy)
            {
                refIntList.Add(new RefInt(item));
            }
        }

        public List<int> ToNormalIntList()
        {
            var result = new List<int>();
            foreach (var item in refIntList)
            {
                result.Add(item.refint);
            }

            return result;
        }

    }
}