using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    public class OddEvenSorting
    {
        public double sequentialTime { get; set; }
        public double parallelTime { get; set; }
        public double referentialTime { get; set; }
        public double quickTime { get; set; }

        private int _n;
        private int multiplication = 3;
        private List<int> _primalColletion;
        private RefIntList _primalRefColletion;
        private List<int> _sortedSequentially;
        private List<int> _sortedParallely;
        private RefIntList _sortedRefParallely;
        private Stopwatch stopWatch;
        private int pararellDegree = 4;
        
        public OddEvenSorting(int n)
        {
            //if(IsOddSeq(n))
            //        throw new Exception($"n = {n} must be an even number");

            _n = n;
            initializeCollection();
        }


        public List<int> GetPrimalColletion()
        {
            return _primalColletion;
        }
        public List<int> GetSortedSequentially()
        {
            if(_sortedSequentially == null)
                sortSequentially();

            return _sortedSequentially;
        }

        public List<int> GetSortedParallely()
        {
            if (_sortedParallely == null)
                sortParallely();

            return _sortedParallely;
        }

        public RefIntList GetSortedRefParallely()
        {
            if (_sortedRefParallely == null)
                sortRefParallely();
            return _sortedRefParallely;
        }

        public void DoitQuick()
        {
            stopWatch = Stopwatch.StartNew();

            var quick = _primalColletion.OrderBy(x=>x).ToList();

            stopWatch.Stop();
            quickTime = stopWatch.Elapsed.TotalMilliseconds;

        }
        

        public void initializeCollection()
        {
            var rand = new Random();

            var collection = new List<int>();

            var iteration = 0;
            while (iteration < _n)
            {
                collection.Add(rand.Next(_n * multiplication));
                iteration +=1;
            }
            _primalRefColletion = new RefIntList(collection);
            _primalColletion = collection;
        }

        private void sortSequentially()
        {
            stopWatch = Stopwatch.StartNew();
            _sortedSequentially = new List<int>(_primalColletion);

            var iteration = 1;
            while (iteration <= _n)
            {
                if (IsOddSeq(iteration)){
                   sortSeq(_sortedSequentially, isEven: 0);
                } else {
                   sortSeq(_sortedSequentially, isEven: 1);
                }

                iteration += 1;
            }

            stopWatch.Stop();
            sequentialTime = stopWatch.Elapsed.TotalMilliseconds;
        }

        private void sortParallely ()
        {
            stopWatch = Stopwatch.StartNew();

            _sortedParallely = new List<int>(_primalColletion);
            var batches = new List<List<int>>();

            int first;
            int last;

            var iteration = 1;
            while (iteration <= _n)
            {
                
                if (IsOddSeq(iteration))
                {
                    batches = splitCollection(_sortedParallely, _n / pararellDegree);
                    Parallel.ForEach(batches, 
                        new ParallelOptions { MaxDegreeOfParallelism = pararellDegree },
                        batch => batch = sortParallel(batch));

                    
                    _sortedParallely.Clear();
                    foreach (var batch in batches)
                    {
                        _sortedParallely.AddRange(batch);
                    }
                }
                else
                {
                    first = _sortedParallely.FirstOrDefault();
                    _sortedParallely.RemoveAt(0);
                    last = _sortedParallely.LastOrDefault();
                    _sortedParallely.RemoveAt(_n-2);

                    batches = splitCollection(_sortedParallely, _n / pararellDegree);
                    Parallel.ForEach(batches,
                        new ParallelOptions { MaxDegreeOfParallelism = pararellDegree }, 
                        batch => batch = sortParallel(batch));

                    _sortedParallely.Clear();
                    _sortedParallely.Add(first);
                    foreach (var batch in batches)
                    {
                        _sortedParallely.AddRange(batch);
                    }
                    _sortedParallely.Add(last);
                }

                iteration += 1;
            }

            stopWatch.Stop();
            parallelTime = stopWatch.Elapsed.TotalMilliseconds;
        }

        private void sortRefParallely()
        {
            _sortedRefParallely = new RefIntList(_primalRefColletion);

            var oddBatches = new List<List<RefInt>>();
            oddBatches.Add(_sortedRefParallely.refIntList.GetRange(0, _n / 4));
            oddBatches.Add(_sortedRefParallely.refIntList.GetRange(_n / 4, _n / 4));
            oddBatches.Add(_sortedRefParallely.refIntList.GetRange(_n / 2, _n / 4));
            oddBatches.Add(_sortedRefParallely.refIntList.GetRange(_n * 3 / 4, _n / 4));

            var evenBatches = new List<List<RefInt>>();
            evenBatches.Add(_sortedRefParallely.refIntList.GetRange(1, _n / 4));
            evenBatches.Add(_sortedRefParallely.refIntList.GetRange((_n / 4) + 1, _n / 4));
            evenBatches.Add(_sortedRefParallely.refIntList.GetRange((_n / 2) + 1, _n / 4));
            evenBatches.Add(_sortedRefParallely.refIntList.GetRange((_n * 3 / 4) + 1, (_n / 4) - 2));

            stopWatch = Stopwatch.StartNew();

            var iteration = 1;
            while (iteration <= _n)
            {
                if (IsOddSeq(iteration))
                {
                   Parallel.ForEach(oddBatches,
                       new ParallelOptions { MaxDegreeOfParallelism = pararellDegree },
                       batch => sortRefParallel(batch));
                }
                else
                {
                    Parallel.ForEach(evenBatches,
                        new ParallelOptions { MaxDegreeOfParallelism = pararellDegree },
                        batch => sortRefParallel(batch));

                }
            
                iteration += 1;
            }

            stopWatch.Stop();
            referentialTime = stopWatch.Elapsed.TotalMilliseconds;
        }


        private void sortSeq(List<int> collection, int isEven = 0)
        {
            var index = 0;
            while (index < collection.Count - 2 * isEven)
            {
                if (index == collection.Count - 1)
                    break;

                if (collection[index + 1] > collection[index + 1 + isEven])
                {
                    int temp = collection[index + 1];
                    collection[index + 1] = collection[index + 1 + isEven];
                    collection[index + 1 + isEven] = temp;
                }

                index += 2;
            }
        }

        private List<int> sortParallel(List<int> collection)
        {
            var index = 0;
            while (index < (collection.Count))
            {
                if (index == collection.Count - 1)
                    return collection;

                if (collection[index] > collection[index + 1])
                {
                    int temp = collection[index];
                    collection[index] = collection[index + 1];
                    collection[index + 1] = temp;
                }

                index += 2;
            }

            return collection;
        }

        private void sortRefParallel(List<RefInt> collection)
        {
            var index = 0;
            while (index < (collection.Count))
            {
                if (index == collection.Count - 1)
                    break;

                if (collection[index].refint > collection[index + 1].refint)
                {
                    int temp = collection[index].refint;
                    collection[index].refint = collection[index + 1].refint;
                    collection[index + 1].refint = temp;
                }

                index += 2;
            }

        }


        private bool IsOddSeq(int iteration)
        {
            return iteration % 2 == 1;
        }

        private List<List<T>> splitCollection<T>(List<T> collection, int size)
        {
            var chunks = new List<List<T>>();
            var chunkCount = collection.Count() / size;

            if (collection.Count % size > 0)
                chunkCount++;

            for (var i = 0; i < chunkCount; i++)
                chunks.Add(collection.Skip(i * size).Take(size).ToList());

            return chunks;
        }

    }
}
