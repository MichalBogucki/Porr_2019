using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    public class OddEvenSorting
    {
        public double SequentialTime { get; set; }
        public double ParallelTime { get; set; }
        public double QuickTime { get; set; }

        private readonly int _n;
        private readonly int multiplication = 3;
        private List<int> _primalColletion;
        private List<int> _primalQuickSorktedColletion;
        private RefIntList _sortedSequentially;
        private RefIntList _sortedParallely;
        private Stopwatch stopWatch;
        private int pararellDegree = 4;
        
        public OddEvenSorting(int n)
        {
            _n = n;
            InitializeCollection();
            DoitQuick();
        }

        public List<int> GetPrimalColletion() { return _primalColletion; }

        public List<int> GetPrimalQuickSorktedColletion() { return _primalQuickSorktedColletion; }
        public RefIntList GetSortedSequentially()
        {
            if(_sortedSequentially == null)
                SortSequentially();
            return _sortedSequentially;
        }
        
        public RefIntList GetSortedParallely()
        {
            if (_sortedParallely == null)
                SortParallely();
            return _sortedParallely;
        }

        public void InitializeCollection()
        {
            var rand = new Random();
            var collection = new List<int>();

            var iteration = 0;
            while (iteration < _n)
            {
                collection.Add(rand.Next(_n * multiplication));
                iteration +=1;
            }
            _primalColletion = collection;
        }

        private void SortSequentially()
        {
            _sortedSequentially = new RefIntList(_primalColletion);
            var oddBatches = InitializeBatches(_sortedSequentially, isEven: 0);
            var evenBatches = InitializeBatches(_sortedSequentially, isEven: 1);

            stopWatch = Stopwatch.StartNew();
            var iteration = 1;
            while (iteration <= _n)
            {
                if (IsOdd(iteration)){
                    foreach (var batch in oddBatches){
                        SwapPhase(batch);
                        //Console.WriteLine("Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
                    }
                } else {
                    foreach (var batch in evenBatches){
                        SwapPhase(batch);
                        //Console.WriteLine("Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
                    }
                }
                iteration += 1;
            }
            stopWatch.Stop();
            SequentialTime = stopWatch.Elapsed.TotalMilliseconds;
        }


        private void SortParallely()
        {
            _sortedParallely = new RefIntList(_primalColletion);
            var oddBatches = InitializeBatches(_sortedParallely, isEven: 0);
            var evenBatches = InitializeBatches(_sortedParallely, isEven: 1);

            stopWatch = Stopwatch.StartNew();
            var iteration = 1;
            while (iteration <= _n) {
                if (IsOdd(iteration)) {
                   Parallel.ForEach(oddBatches,
                       new ParallelOptions { MaxDegreeOfParallelism = pararellDegree },
                       batch => {
                           SwapPhase(batch);
                           //Console.WriteLine("Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
                       });
                } else {
                    Parallel.ForEach(evenBatches,
                        new ParallelOptions { MaxDegreeOfParallelism = pararellDegree },
                        batch => {
                            SwapPhase(batch);
                            //Console.WriteLine("Thread Id= {0}", Thread.CurrentThread.ManagedThreadId);
                        });
                }
                iteration += 1;
            }
            stopWatch.Stop();
            ParallelTime = stopWatch.Elapsed.TotalMilliseconds;
        }

        private ConcurrentBag<List<RefInt>> InitializeBatches(RefIntList collection, int isEven)
        {
            var count = _n / 4;
            if (IsOdd(count))
                throw new Exception($"Batch.Count = {count} must be an even number.");

            var evenBatches = new ConcurrentBag<List<RefInt>>
            {
                collection.refIntList.GetRange(isEven, count),
                collection.refIntList.GetRange((1 * count) + isEven, count),
                collection.refIntList.GetRange((2 * count) + isEven, count),
                collection.refIntList.GetRange((3 * count) + isEven, (count) - 2 * isEven)
            };
            return evenBatches;
        }

        private bool IsOdd(int iteration)
        {
            return iteration % 2 == 1;
        }

        private void SwapPhase(List<RefInt> batch)
        {
            if (IsOdd(batch.Count))
                throw new Exception($"Batch.Count = {batch.Count} must be an even number.");

            var index = 0;
            while (index < (batch.Count))
            {
                if (index == batch.Count - 1)
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

        private void DoitQuick()
        {
            stopWatch = Stopwatch.StartNew();

            _primalQuickSorktedColletion = _primalColletion.OrderBy(x => x).ToList();

            stopWatch.Stop();
            QuickTime = stopWatch.Elapsed.TotalMilliseconds;
        }

    }
}
