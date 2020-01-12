namespace HybridizerSample1
{
    public class  RefIntList
    {
        private static int N;
        public RefInt[] refIntList;// { get; set; } = new RefInt[N];
        
        public RefIntList(RefIntList refIntListToCopy, int n)
        {
            N = n;

            refIntList = new RefInt[N];
            for (int i = 0; i < N; ++i)
            {
                refIntList[i] = new RefInt(refIntListToCopy.refIntList[i].refint);
            }
        }

        public RefIntList(RefInt[] refIntListToCopy, int n)
        {
            N = n;

            refIntList = new RefInt[N];
            for (int i = 0; i < N; ++i)
            {
                refIntList[i] = new RefInt(refIntListToCopy[i].refint);
            }
        }

        public RefIntList(int[] intListToCopy, int n)
        {
            N = n;
            refIntList = new RefInt[N];
            for (int i = 0; i < N; ++i)
            {
                refIntList[i] = new RefInt(intListToCopy[i]);
            }
        }

        public int[] ToNormalIntList()
        {
            var result = new int[N];
            for (int i = 0; i < N; i++)
            {
                result[i]=refIntList[i].refint;
            }

            return result;
        }

    }
}