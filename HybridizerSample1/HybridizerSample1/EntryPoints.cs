using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hybridizer.Runtime.CUDAImports;

namespace HybridizerSample1
{
    public class EntryPoints
    {
        [EntryPoint("run")]
        private static void SortParallely(
            RefInt[] oddBatch0, RefInt[] oddBatch1, RefInt[] oddBatch2, RefInt[] oddBatch3, 
            RefInt[] evenBatch0, RefInt[] evenBatch1, RefInt[] evenBatch2, RefInt[] evenBatch3,
            int oddLength0, int oddLength1,int oddLength2, int oddLength3,
            int evenLength0, int evenLength1, int evenLength2, int evenLength3,
            int _n)
        {

            for (int iteration = 1; iteration <= _n; iteration++)
            {
                if (iteration % 2 == 1)
                {
                    Parallel.For(0, 4, i =>
                    {
                        if (i == 0)
                        {
                            SwapPhase(oddBatch0, 1, oddLength0);
                        }
                        if (i == 1)
                        {
                            SwapPhase(oddBatch1, 1, oddLength1);
                        }
                        if (i == 2)
                        {
                            SwapPhase(oddBatch2, 1, oddLength2);
                        }
                        if (i == 3)
                        {
                            SwapPhase(oddBatch3, 1, oddLength3);
                        }
                    });
                }
                else
                {
                    Parallel.For(0, 4, i =>
                    {
                        if (i == 0)
                        {
                            SwapPhase(evenBatch0, 1, evenLength0);
                        }
                        if (i == 1)
                        {
                            SwapPhase(evenBatch1, 1, evenLength1);
                        }
                        if (i == 2)
                        {
                            SwapPhase(evenBatch2, 1, evenLength2);
                        }
                        if (i == 3)
                        {
                            SwapPhase(evenBatch3, 1, evenLength3);
                        }
                    });
                }
            }
        }

        [Kernel]
        public static RefInt[] GetPrimalColletion(RefInt[] _primalColletion)
        {
            return _primalColletion;
        }


        [Kernel]
        private static void SwapPhase(RefInt[] batch, int isParal, int batchLength)
        {
            var thread = isParal == 1 ? "(paral)" : "(seq)";
            var index = 0;
            while (index < (batchLength))
            {
                if (index == batchLength - 1)
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



        //###########################################################################

        static void Main(string[] args)
        {
            cudaDeviceProp prop;
            cuda.GetDeviceProperties(out prop, 0);
            var runner = HybRunner.Cuda().SetDistrib(prop.multiProcessorCount * 16, 128);

            var entryPoints = new EntryPoints();
            
            int n = 1024;
            Console.WriteLine($"Please provide number \"n\":");
            var consoleInput = Console.ReadLine();
            bool success = int.TryParse(consoleInput, out int consoleOutput);
            if (success)
                n = consoleOutput;



            //################################
            double ParallelTime = 0;
            int _n = n;
            int multiplication = 3;
            RefInt[] _primalColletion = new RefInt[] { };
            RefIntList _sortedParallely = null;
            Stopwatch stopWatch = null;
            //################################

            //-------- InitializeColleciton  --------
            if (_n % 2 == 1)
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
            //-------- InitializeCollection  --------
            
            var primalColletion = GetPrimalColletion(_primalColletion);


            //----------InitializeBatches---------------
            _sortedParallely = new RefIntList(_primalColletion, _primalColletion.Length);

                
            //++++++++++++++ OddBatches +++++++++++++++
            var moduloFourSlide = 0;
            if (_n % 4 == 2)
                moduloFourSlide = 2;
            var oddFloor = 0;
            var floor = (int)Math.Floor(_n / 4.0);
            if (floor % 2 == 1)
                oddFloor = 1; ;

            //----------------------GetRange()----------------------------
            var oddLength0 = floor + oddFloor;
            var oddBatch0 = new RefInt[oddLength0];
            for (int i = 0; i < oddLength0; i++)
            {
                oddBatch0[i] = _sortedParallely.refIntList[0 + i];
            }
            //----------------------GetRange()----------------------------
            var oddLength1 = floor + moduloFourSlide + oddFloor;
            var oddBatch1 = new RefInt[oddLength1];
            for (int i = 0; i < oddLength1; i++)
            {
                oddBatch1[i] = _sortedParallely.refIntList[(1 * floor + oddFloor) + 0 + i];
            }
            //----------------------GetRange()----------------------------
            var oddLength2 = floor - oddFloor;
            var oddBatch2 = new RefInt[floor - oddFloor];
            for (int i = 0; i < floor - oddFloor; i++)
            {
                oddBatch2[i] = _sortedParallely.refIntList[(2 * floor + moduloFourSlide + oddFloor * 2) + 0 + i];
            }
            //----------------------GetRange()----------------------------
            var oddLength3 = (floor) - 2 * 0 - oddFloor;
            var oddBatch3 = new RefInt[(floor) - 2 * 0 - oddFloor];
            for (int i = 0; i < (floor) - 2 * 0 - oddFloor; i++)
            {
                oddBatch3[i] = _sortedParallely.refIntList[(3 * floor + moduloFourSlide + oddFloor) + 0 + i];
            }
            //----------------------GetRange()----------------------------
            //++++++++++++++ OddBatches +++++++++++++++


            //++++++++++++++ EvenBatches +++++++++++++++
             moduloFourSlide = 0;
            if (_n % 4 == 2)
                moduloFourSlide = 2;
             oddFloor = 0;
             floor = (int)Math.Floor(_n / 4.0);
            if (floor % 2 == 1)
                oddFloor = 1; ;

            //----------------------GetRange()----------------------------
            var evenLength0 = floor + oddFloor;
            var evenBatch0 = new RefInt[floor + oddFloor];
            for (int i = 0; i < floor + oddFloor; i++)
            {
                evenBatch0[i] = _sortedParallely.refIntList[1 + i];
            }
            //----------------------GetRange()----------------------------
            var evenLength1 = floor + moduloFourSlide + oddFloor;
            var evenBatch1 = new RefInt[floor + moduloFourSlide + oddFloor];
            for (int i = 0; i < floor + moduloFourSlide + oddFloor; i++)
            {
                evenBatch1[i] = _sortedParallely.refIntList[(1 * floor + oddFloor) + 1 + i];
            }
            //----------------------GetRange()----------------------------
            var evenLegth2 = floor - oddFloor;
            var evenBatch2 = new RefInt[floor - oddFloor];
            for (int i = 0; i < floor - oddFloor; i++)
            {
                evenBatch2[i] = _sortedParallely.refIntList[(2 * floor + moduloFourSlide + oddFloor * 2) + 1 + i];
            }
            //----------------------GetRange()----------------------------
            var evenLength3 = (floor) - 2 * 1 - oddFloor;
            var evenBatch3 = new RefInt[(floor) - 2 * 1 - oddFloor];
            for (int i = 0; i < (floor) - 2 * 1 - oddFloor; i++)
            {
                evenBatch3[i] = _sortedParallely.refIntList[(3 * floor + moduloFourSlide + oddFloor) + 1 + i];
            }
            //----------------------GetRange()----------------------------
            //++++++++++++++ EvenBatches +++++++++++++++




            //----------InitializeBatches---------------




            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            var wrapped = runner.Wrap(entryPoints);
            //----StopWatch----

            stopWatch = Stopwatch.StartNew();
            
            SortParallely(
                oddBatch0, oddBatch1, oddBatch2, oddBatch3, 
                evenBatch0, evenBatch1, evenBatch2, evenBatch3,
                oddLength0,oddLength1,oddLength2,oddLength3,
                evenLength0,evenLength1,evenLegth2,evenLength3,
                _n); //On CPU

            //wrapped.SortParallely(
            //    oddBatch0, oddBatch1, oddBatch2, oddBatch3,
            //    evenBatch0, evenBatch1, evenBatch2, evenBatch3,
            //    oddLength0, oddLength1, oddLength2, oddLength3,
            //    evenLength0, evenLength1, evenLegth2, evenLength3,
            //    _n); //On GPU
            
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            stopWatch.Stop();
            ParallelTime = stopWatch.Elapsed.TotalMilliseconds;
            //----StopWatch----
            
            var pararellReslutList = _sortedParallely.ToNormalIntList();
            
            
            if (n <= 16)
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"{primalColletion[i].refint}");
                }
            
            Console.WriteLine($"n = {n}, parallelTime = {ParallelTime} ms");
            
            if (n <= 16)
                for (int i = 0; i<n; i++)
                {
                    Console.WriteLine($"{_sortedParallely.refIntList[i].refint}");
                }
        }

        //#############################################################



    }
}