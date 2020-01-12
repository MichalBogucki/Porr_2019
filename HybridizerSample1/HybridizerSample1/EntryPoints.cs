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

        [EntryPoint("run")]
        public static void Run()
        {

        }

        [Kernel]
        public static RefInt[] InitializeCollection(int _n, int multiplication, RefInt[] _primalColletion)
        {
            if (IsOdd(_n))
                throw new Exception($"n = {_n} must be an even number.");

            var rand = new Random();
            var collection = new RefInt[_n];

            var iteration = 0;
            while (iteration < _n)
            {
                collection[iteration] = new RefInt(rand.Next(_n * multiplication));
                iteration += 1;
            }
            _primalColletion = collection;
            return _primalColletion;
        }
        
        [Kernel]
        public static RefInt[] GetPrimalColletion(RefInt[] _primalColletion) { return _primalColletion; }


        public static RefIntList GetSortedParallely(RefIntList _sortedParallely, RefInt[] _primalColletion, Stopwatch stopWatch, int parallelDegree, double ParallelTime, int _n)
        {
            if (_sortedParallely == null)
                _sortedParallely = SortParallely(_sortedParallely, _primalColletion, stopWatch, parallelDegree, ParallelTime,  _n);
            return _sortedParallely;
        }

        

        [Kernel]
        private static RefIntList SortParallely(RefIntList _sortedParallely, RefInt[] _primalColletion, Stopwatch stopWatch, int parallelDegree, double ParallelTime,  int _n)
        {
            _sortedParallely = new RefIntList(_primalColletion, _primalColletion.Length);
            var oddBatches = InitializeConcurrentBatches(_sortedParallely, _n, isEven: 0);
            var evenBatches = InitializeConcurrentBatches(_sortedParallely, _n, isEven: 1);

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
            _sortedParallely.time = ParallelTime;
            return _sortedParallely;
        }

        [Kernel]
        private static RefInt[][] InitializeConcurrentBatches(RefIntList collection, int _n, int isEven)
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


        [Kernel]
        private static bool IsOdd(int iteration)
        {
            return iteration % 2 == 1;
        }

        [Kernel]
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

        [Kernel]
        private static RefInt[] GetRange(RefInt[] refIntList, int index, int count)
        {
            var tempCopy = new RefInt[count];
            for (int i = 0; i < count; i++)
            {
                tempCopy[i] = refIntList[index + i];
            }

            return tempCopy;
        }


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
            

            // run the method on GPU
            Console.WriteLine($"--GPU STARTED");
            //wrapped.OddEvenSorting(N, acuda, adotnet); >> ToDO UNCOMMENT ME <<<
            Console.WriteLine($"--GPU finished");


            //-------------------------------------
            int n = 16;
            Console.WriteLine($"Please provide number \"n\"=100");
            //var consoleInput = Console.ReadLine();
            //bool success = int.TryParse(consoleInput, out int consoleOutput);
            //if (success)
            //    n = consoleOutput;



            //################################
            double ParallelTime = 0;
            int _n = n;
            int multiplication = 3;
            RefInt[] _primalColletion = new RefInt[] { };
            RefIntList _sortedParallely = null;
            Stopwatch stopWatch = null;
            int parallelDegree = 4;
            //################################

            var wrapped = runner.Wrap(entryPoints);

            _primalColletion = InitializeCollection(_n, multiplication, _primalColletion);
            
            //_primalColletion = wrapped.OddEvenSorting(n, _n, multiplication, _primalColletion);

            var primalColletion = GetPrimalColletion(_primalColletion);

            var parallelResult = GetSortedParallely( _sortedParallely, _primalColletion,  stopWatch, parallelDegree,  ParallelTime, _n);
            ParallelTime = parallelResult.time;

            var pararellReslutList = parallelResult.ToNormalIntList();


            if (n <= 16)
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"{primalColletion[i].refint}");
                }

            Console.WriteLine($"n = {n}, parallelTime = {ParallelTime} ms");

            if (n <= 16)
                for (int i = 0; i<n; i++)
                {
                    Console.WriteLine($"{parallelResult.refIntList[i].refint}");
                }

            Console.WriteLine($"Press any key to exit.");
            Console.ReadLine();
        }

        //#############################################################



    }
}