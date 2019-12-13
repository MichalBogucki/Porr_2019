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

        private int _n;
        private int multiplication = 3;
        private List<int> _primalColletion;
        private List<int> _sortedSequentially;
        private List<int> _sortedParallely;
        private Stopwatch stopWatch;
        private int pararellDegree = 4;
        

        public List<int> GetPrimalColletion()
        {
            return _primalColletion;
        }
        public List<int> GetSortedSequentially()
        {
            return _sortedSequentially;
        }


        public List<int> GetSortedParallely()
        {
            return _sortedParallely;
        }

        public OddEvenSorting(int n)
        {
            if(IsOddSeq(n))
                throw new Exception($"n = {n} must be an even number");

            _n = n;
            _primalColletion = initializeCollection();

            stopWatch = Stopwatch.StartNew();
            _sortedSequentially = sortSequentially(_primalColletion);
            stopWatch.Stop();
            sequentialTime = stopWatch.Elapsed.TotalMilliseconds;

            stopWatch = Stopwatch.StartNew();
            _sortedParallely = sortParallely(_primalColletion);
            stopWatch.Stop();
            parallelTime = stopWatch.Elapsed.TotalMilliseconds;

        }

        public List<int> initializeCollection()
        {
            var rand = new Random();

            var collection = new List<int>();

            var iteration = 0;
            while (iteration < _n)
            {
                collection.Add(rand.Next(_n * multiplication));
                iteration+=1;
            }

            return collection;
        }

        private List<int> sortSequentially(List<int> collection)
        {
            var tempCollection = new List<int>(collection);

            var iteration = 1;
            while (iteration <= _n)
            {
                if (IsOddSeq(iteration)){
                    tempCollection = sortOddSeq(tempCollection);
                } else {
                    tempCollection = sortEvenSeq(tempCollection);
                }

                iteration += 1;
            }

            return tempCollection;
        }

        private List<int> sortParallely (List<int> collection)
        {
            var tempCollection = new List<int>(collection);
            var batches = new List<List<int>>();
            int first;
            int last;

            var iteration = 1;
            while (iteration <= _n)
            {
                
                if (IsOddSeq(iteration))
                {
                    batches = splitCollection(tempCollection, _n / pararellDegree);
                    Parallel.ForEach(batches, batch => batch = sortOddParal(batch));

                    tempCollection = new List<int>();
                    foreach (var batch in batches)
                    {
                        tempCollection.AddRange(batch);
                    }
                }
                else
                {
                    first = tempCollection.FirstOrDefault();
                    tempCollection.RemoveAt(0);
                    last = tempCollection.LastOrDefault();
                    tempCollection.RemoveAt(_n-2);

                    batches = splitCollection(tempCollection, _n / pararellDegree);
                    Parallel.ForEach(batches, batch => batch = sortEvenParal(batch));

                    tempCollection = new List<int>();
                    tempCollection.Add(first);
                    foreach (var batch in batches)
                    {
                        tempCollection.AddRange(batch);
                    }
                    tempCollection.Add(last);
                }

                
                

                iteration += 1;
            }


            //Todo Task.AwaitAll();
            return tempCollection;
        }


        private List<int> sortOddSeq(List<int> collection)
        {
            var index = 0;
            while (index < collection.Count)
            {
                var left = collection[index];
                var right = collection[index + 1];
                if (left > right)
                {
                    int temp = collection[index];
                    collection[index] = collection[index + 1];
                    collection[index + 1] = temp;
                }

                index += 2;
            }

            return collection;
        }

        private List<int> sortEvenSeq(List<int> collection)
        {
            var index = 0;
            while (index < collection.Count - 2)
            {
                var left  = collection[index + 1];
                var right = collection[index + 2];
                if (left > right)
                {
                    int temp = collection[index + 1];
                    collection[index + 1] = collection[index + 2];
                    collection[index + 2] = temp;
                }

                index += 2;
            }

            return collection;
        }

        private List<int> sortEvenParal(List<int> collection)
        {
            var index = 0;
            while (index < (collection.Count))
            {
                var left = collection[index];
                var right = collection[index + 1];
                if (left > right)
                {
                    int temp = collection[index];
                    collection[index] = collection[index + 1];
                    collection[index + 1] = temp;
                }

                index += 2;
            }

            return collection;
        }
        private List<int> sortOddParal(List<int> collection)
        {
            var index = 0;
            while (index < collection.Count)
            {
                var left = collection[index];
                var right = collection[index + 1];
                if (left > right)
                {
                    int temp = collection[index];
                    collection[index] = collection[index + 1];
                    collection[index + 1] = temp;
                }

                index += 2;
            }

            return collection;
        }


        private bool IsOddSeq(int iteration)
        {
            return iteration % 2 == 1;
        }

        public List<List<T>> splitCollection<T>(List<T> collection, int size)
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
