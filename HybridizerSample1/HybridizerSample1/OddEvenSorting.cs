//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Hybridizer.Runtime.CUDAImports;

//namespace HybridizerSample1
//{
//    public class OddEvenSorting
//    {
//        public double SequentialTime { get; set; }
//        public double ParallelTime { get; set; }
//        public double QuickTime { get; set; }

//        private readonly int _n;
//        private readonly int multiplication = 3;
//        private int[] _primalColletion;
//        private int[] _primalQuickSorktedColletion;
//        private RefIntList _sortedParallely;
//        private Stopwatch stopWatch;
//        private int parallelDegree = 4;

//        [Kernel]
//        public OddEvenSorting(int n)
//        {
//            if (IsOdd(n))
//                throw new Exception($"n = {n} must be an even number.");
//            _n = n;
//            InitializeCollection();
//        }

//        [Kernel]
//        public int[] GetPrimalColletion() { return _primalColletion; }

//        [Kernel]
//        public int[] GetPrimalQuickSortedColletion() { return _primalQuickSorktedColletion; }
        
//        public RefIntList GetSortedParallely()
//        {
//            if (_sortedParallely == null)
//                SortParallely();
//            return _sortedParallely;
//        }

//        [Kernel]
//        public void InitializeCollection()
//        {
//            var rand = new Random();
//            var collection = new int[_n];

//            var iteration = 0;
//            while (iteration < _n)
//            {
//                collection[iteration]=(rand.Next(_n * multiplication));
//                iteration +=1;
//            }
//            _primalColletion = collection;
//        }

//        [Kernel]
//        private void SortParallely()
//        {
//            _sortedParallely = new RefIntList(_primalColletion, _primalColletion.Length);
//            var oddBatches = InitializeConcurrentBatches(_sortedParallely, isEven: 0);
//            var evenBatches = InitializeConcurrentBatches(_sortedParallely, isEven: 1);

//            stopWatch = Stopwatch.StartNew();
//            var iteration = 1;
//            while (iteration <= _n) {
//                if (IsOdd(iteration)) {
//                   Parallel.ForEach(oddBatches,
//                       new ParallelOptions { MaxDegreeOfParallelism = parallelDegree },
//                       batch => {
//                           SwapPhase(batch, isParal:1);
//                           //Console.WriteLine("(paral)Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
//                       });
//                } else {
//                    Parallel.ForEach(evenBatches,
//                        new ParallelOptions { MaxDegreeOfParallelism = parallelDegree },
//                        batch => {
//                            SwapPhase(batch, isParal: 1);
//                            //Console.WriteLine("(paral)Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
//                        });
//                }
//                iteration += 1;
//            }
//            stopWatch.Stop();
//            ParallelTime = stopWatch.Elapsed.TotalMilliseconds;
//        }

//        [Kernel]
//        private RefInt[][] InitializeConcurrentBatches(RefIntList collection, int isEven)
//        {
//            var moduloFourSlide = 0;
//            if (_n % 4 == 2)
//                moduloFourSlide = 2;
//            var oddFloor = 0;
//            var floor = (int)Math.Floor(_n / 4.0);
//            if (IsOdd(floor))
//                oddFloor = 1; ;

//            var concurrentBatches = new RefInt[4][];

//            concurrentBatches[0] = GetRange(collection.refIntList, isEven, floor + oddFloor);
//            concurrentBatches[1] = GetRange(collection.refIntList, (1 * floor + oddFloor) + isEven, floor + moduloFourSlide + oddFloor);
//            concurrentBatches[2] = GetRange(collection.refIntList, (2 * floor + moduloFourSlide + oddFloor * 2) + isEven, floor - oddFloor);
//            concurrentBatches[3] = GetRange(collection.refIntList, (3 * floor + moduloFourSlide + oddFloor) + isEven, (floor) - 2 * isEven - oddFloor);

//            return concurrentBatches;
//        }


//        [Kernel]
//        private bool IsOdd(int iteration)
//        {
//            return iteration % 2 == 1;
//        }

//        [Kernel]
//        private void SwapPhase(RefInt[] batch, int isParal)
//        {
//            var thread = isParal == 1 ? "(paral)" : "(seq)";
//            var index = 0;
//            while (index < (batch.Length))
//            {
//                //Console.WriteLine($"{thread}Thread Id= {Thread.CurrentThread.ManagedThreadId}");
//                if (index == batch.Length - 1)
//                    break;

//                if (batch[index].refint > batch[index + 1].refint)
//                {
//                    int temp = batch[index].refint;
//                    batch[index].refint = batch[index + 1].refint;
//                    batch[index + 1].refint = temp;
//                }
//                index += 2;
//            }
//        }

//        [Kernel]
//        private RefInt[] GetRange(RefInt[] refIntList, int index, int count)
//        {
//            var tempCopy = new RefInt[count];
//            for (int i = 0; i < count; i++)
//            {
//                tempCopy[i]=refIntList[index + i];
//            }

//            return tempCopy;
//        }

//    }
//}
