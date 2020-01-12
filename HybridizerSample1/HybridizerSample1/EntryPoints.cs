using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hybridizer.Runtime.CUDAImports;

namespace HybridizerSample1
{
    public class EntryPoints
    {

        //[EntryPoint("run")]
        //public static void Run(int N, RefInt[] acuda, RefInt[] adotnet)
        //{
        //    //to make GPU USAGE
        //    Parallel.For(0, N, i =>
        //    {
        //        adotnet[i].refint = ((acuda[i].refint) + 100);
        //        Minus5(N, acuda, adotnet);
        //    });
        //}
        //
        //[Kernel]
        //public static void Minus5(int N, RefInt[] acuda, RefInt[] adotnet)
        //{
        //    //to make GPU USAGE
        //    Parallel.For(0, N, i => { acuda[i].refint = ((acuda[i].refint) - 5); });
        //}


        //###########################################################################

        static void Main(string[] args)
        {
            // 268 MB allocated on device -- should fit in every CUDA compatible GPU
            //int N = 1024 * 1024 * 16;
            int N = 8;
            var acuda = new RefInt[N];
            var adotnet = new RefInt[N];
            var b = new RefInt[N];

            //Initialize acuda et adotnet and b by some doubles randoms, acuda and adotnet have same numbers. 
            for (int i = 0; i < N; ++i)
            {
                acuda[i]= new RefInt(i);
                adotnet[i]= new RefInt(i);
                b[i]= adotnet[i];
            }
            
            cudaDeviceProp prop;
            cuda.GetDeviceProperties(out prop, 0);
            var runner = HybRunner.Cuda().SetDistrib(prop.multiProcessorCount * 16, 128);

            // create a wrapper object to call GPU methods instead of C#
            var entryPoints = new EntryPoints();
            var wrapped = runner.Wrap(entryPoints);

            // run the method on GPU
            Console.WriteLine($"--GPU STARTED");
            //wrapped.Run(N, acuda, adotnet); >> ToDO UNCOMMENT ME <<<
            Console.WriteLine($"--GPU finished");


            //-------------------------------------
            int n = 100;
            Console.WriteLine($"Please provide number \"n\"=100");
            //var consoleInput = Console.ReadLine();
            //bool success = int.TryParse(consoleInput, out int consoleOutput);
            //if (success)
            //    n = consoleOutput;

            OddEvenSorting(n);
            //wrapped.OddEvenSorting(n);

            var primalColletion = GetPrimalColletion();

            var parallelResult = GetSortedParallely().ToNormalIntList();

            var sequentialTime = SequentialTime;
            var parallelTime = ParallelTime;

            if (n <= 16)
                Console.WriteLine(string.Join(", ", primalColletion.ToArray()));

            Console.WriteLine($"n = {n}, parallelTime = {parallelTime} ms");

            if (n <= 16)
            {
                Console.WriteLine(string.Join(", ", parallelResult.ToArray()));
            }

            Console.WriteLine($"Press any key to exit.");
            Console.ReadLine();
        }

        //#############################################################


        public static double SequentialTime { get; set; }
        public static double ParallelTime { get; set; }
        public double QuickTime { get; set; }

        private static int _n;
        private static readonly int multiplication = 3;
        private static int[] _primalColletion;
        private int[] _primalQuickSorktedColletion;
        private static RefIntList _sortedParallely;
        private static Stopwatch stopWatch;
        private static int parallelDegree = 4;

        ///[EntryPoint("run")]
        public static void OddEvenSorting(int n)
        {
            if (IsOdd(n))
                throw new Exception($"n = {n} must be an even number.");
            _n = n;
            InitializeCollection();
        }

        //[EntryPoint("getPrimalColletion")]
        public static int[] GetPrimalColletion() { return _primalColletion; }


        public static RefIntList GetSortedParallely()
        {
            if (_sortedParallely == null)
                SortParallely();
            return _sortedParallely;
        }

        //[Kernel]
        public static void InitializeCollection()
        {
            var rand = new Random();
            var collection = new int[_n];

            var iteration = 0;
            while (iteration < _n)
            {
                collection[iteration] = (rand.Next(_n * multiplication));
                iteration += 1;
            }
            _primalColletion = collection;
        }

        //[Kernel]
        private static void SortParallely()
        {
            _sortedParallely = new RefIntList(_primalColletion, _primalColletion.Length);
            var oddBatches = InitializeConcurrentBatches(_sortedParallely, isEven: 0);
            var evenBatches = InitializeConcurrentBatches(_sortedParallely, isEven: 1);

            stopWatch = Stopwatch.StartNew();
            var iteration = 1;
            while (iteration <= _n)
            {
                if (IsOdd(iteration))
                {
                    Parallel.ForEach(oddBatches,
                        new ParallelOptions { MaxDegreeOfParallelism = parallelDegree },
                        batch => {
                            SwapPhase(batch, isParal: 1);
                           //Console.WriteLine("(paral)Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
                       });
                }
                else
                {
                    Parallel.ForEach(evenBatches,
                        new ParallelOptions { MaxDegreeOfParallelism = parallelDegree },
                        batch => {
                            SwapPhase(batch, isParal: 1);
                            //Console.WriteLine("(paral)Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
                        });
                }
                iteration += 1;
            }
            stopWatch.Stop();
            ParallelTime = stopWatch.Elapsed.TotalMilliseconds;
        }

        //[Kernel]
        private static RefInt[][] InitializeConcurrentBatches(RefIntList collection, int isEven)
        {
            var moduloFourSlide = 0;
            if (_n % 4 == 2)
                moduloFourSlide = 2;
            var oddFloor = 0;
            var floor = (int)Math.Floor(_n / 4.0);
            if (IsOdd(floor))
                oddFloor = 1; ;

            var concurrentBatches = new RefInt[4][];

            concurrentBatches[0] = GetRange(collection.refIntList, isEven, floor + oddFloor);
            concurrentBatches[1] = GetRange(collection.refIntList, (1 * floor + oddFloor) + isEven, floor + moduloFourSlide + oddFloor);
            concurrentBatches[2] = GetRange(collection.refIntList, (2 * floor + moduloFourSlide + oddFloor * 2) + isEven, floor - oddFloor);
            concurrentBatches[3] = GetRange(collection.refIntList, (3 * floor + moduloFourSlide + oddFloor) + isEven, (floor) - 2 * isEven - oddFloor);

            return concurrentBatches;
        }


        //[Kernel]
        private static bool IsOdd(int iteration)
        {
            return iteration % 2 == 1;
        }

        //[Kernel]
        private static void SwapPhase(RefInt[] batch, int isParal)
        {
            var thread = isParal == 1 ? "(paral)" : "(seq)";
            var index = 0;
            while (index < (batch.Length))
            {
                //Console.WriteLine($"{thread}Thread Id= {Thread.CurrentThread.ManagedThreadId}");
                if (index == batch.Length - 1)
                    break;

                if (batch[index].refint > batch[index + 1].refint)
                {
                    int temp = batch[index].refint;
                    batch[index].refint = batch[index + 1].refint;
                    batch[index + 1].refint = temp;
                }
                index += 2;
            }
        }

        //[Kernel]
        private static RefInt[] GetRange(RefInt[] refIntList, int index, int count)
        {
            var tempCopy = new RefInt[count];
            for (int i = 0; i < count; i++)
            {
                tempCopy[i] = refIntList[index + i];
            }

            return tempCopy;
        }

    }
}