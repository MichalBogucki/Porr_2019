using Hybridizer.Runtime.CUDAImports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
    class EntryPoints
    {
       //EntryPoints()
       //{
       //}
       //private static readonly object padlock = new object();
       //private static EntryPoints instance = null;
       //public static EntryPoints Instance
       //{
       //    get
       //    {
       //        if (instance == null)
       //        {
       //            lock (padlock)
       //            {
       //                if (instance == null)
       //                {
       //                    instance = new EntryPoints();
       //                }
       //            }
       //        }
       //        return instance;
       //    }
       //}

        //[EntryPoint("run")]
        //public static void Run(int N, double[] a, double[] b)
        //{
        //    for (int iter = 0; iter < 100; iter++)
        //    {
        //        //to make GPU USAGE
        //        Parallel.For(0, N, i => {
        //            a[i] += b[i];
        //            a[i] -= b[i];
        //            a[i] = 7.0d;
        //        });

        //    }
        //}

        [EntryPoint("run")]
        public static void Run(int N, RefInt[] acuda, RefInt[] adotnet)
        {
            //to make GPU USAGE
            Parallel.For(0, N, i =>
            {
                adotnet[i].refint = ((acuda[i].refint) + 100);
                Minus5(N, acuda, adotnet);
                Minus5(N, acuda, adotnet);
            });
        }

        [Kernel]
        public static void Minus5(int N, RefInt[] acuda, RefInt[] adotnet)
        {
            //to make GPU USAGE
            Parallel.For(0, N, i => { acuda[i].refint = ((acuda[i].refint) - 5); });
        }

        //[EntryPoint("run")]
        //public static void Run(int N, List<RefInt> a, List<RefInt> b)
        //{
        //        Parallel.For(0, N, i =>
        //        {
        //            b[i].refint = a[i].refint + 20;
        //        });
        //}

        static void Main(string[] args)
        {
            // 268 MB allocated on device -- should fit in every CUDA compatible GPU
            //int N = 1024 * 1024 * 16;
            int N = 8;
            var acuda = new RefInt[N];
            var adotnet = new RefInt[N];
            var b = new RefInt[N];

            var list = new List<RefInt>()
            {
                new RefInt(1),
                new RefInt(2),
                new RefInt(3),
                new RefInt(4),
                new RefInt(5),
                new RefInt(6),
                new RefInt(7),
                new RefInt(8),
            };

            var newList = new List<RefInt>(){
                new RefInt(0),
                new RefInt(0),
                new RefInt(0),
                new RefInt(0),
                new RefInt(0),
                new RefInt(0),
                new RefInt(0),
                new RefInt(0),
            }; ;

            

            Random rand = new Random();

            //Initialize acuda et adotnet and b by some doubles randoms, acuda and adotnet have same numbers. 
            for (int i = 0; i < N; ++i)
            {
                acuda[i]= new RefInt(i);
                adotnet[i]= new RefInt(i);
                b[i]= adotnet[i];
                //acuda[i] = rand.NextDouble();
                //adotnet[i] = acuda[i];
                //b[i] = rand.NextDouble();
            }

            
            //var acudaSwap = acuda;

            Console.WriteLine();
            cudaDeviceProp prop;
            cuda.GetDeviceProperties(out prop, 0);
            var runner = HybRunner.Cuda().SetDistrib(prop.multiProcessorCount * 16, 128);

            // create a wrapper object to call GPU methods instead of C#
            var entryPoints = new EntryPoints();
            var wrapped = runner.Wrap(entryPoints);

            // run the method on GPU
            Console.WriteLine($"--GPU STARTED");
            wrapped.Run(N, acuda, adotnet);
            //wrapped.Minus5(N, acuda, adotnet);
            //wrapped.Run(6, list, newList);
            Console.WriteLine($"--GPU finished");

            //var holder = acuda.GetRange(0, N);
            var holderRefInf = newList.GetRange(0, 6);
            
            // run .Net method
            //Run(6, list, newList);
            //Run(N, acuda, adotnet);
            //holder[N-1] = 77.ToString();
            //holderRefInf[5].refint = 77;
            // verify the results
            //for (int k = 0; k < N; ++k)
            b[7].refint = 777;
            for (int k = 0; k < N; ++k)
            {
               Console.WriteLine($"b[k]={b[k].refint}");
               Console.WriteLine($"acuda[k]={acuda[k].refint}");
               Console.WriteLine($"adotnet[k]={adotnet[k].refint}");
               //Console.WriteLine($"holder[k]={holder[k]}");
                Console.WriteLine();
            }
            //for (int k = 0; k < 6; ++k)
            //{
            //    Console.WriteLine($"list[k]={list[k].refint}");
            //    Console.WriteLine($"newlist[k]={newList[k].refint}");
            //    Console.WriteLine($"holderRefInf[k]={holderRefInf[k].refint}");
            //    Console.WriteLine();
            //}


            Console.Out.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}