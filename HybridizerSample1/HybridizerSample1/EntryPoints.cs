using Hybridizer.Runtime.CUDAImports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class EntryPoints
    {

        [EntryPoint("run")]
        public static void Run(int N, RefInt[] acuda, RefInt[] adotnet)
        {
            //to make GPU USAGE
            Parallel.For(0, N, i =>
            {
                adotnet[i].refint = ((acuda[i].refint) + 100);
                Minus5(N, acuda, adotnet);
            });
        }

        [Kernel]
        public static void Minus5(int N, RefInt[] acuda, RefInt[] adotnet)
        {
            //to make GPU USAGE
            Parallel.For(0, N, i => { acuda[i].refint = ((acuda[i].refint) - 5); });
        }


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
            wrapped.Run(N, acuda, adotnet);
            Console.WriteLine($"--GPU finished");
            
            // run .Net method on CPU
            //Run(N, acuda, adotnet);
            //holder[N-1] = 77.ToString();
            //holderRefInf[5].refint = 77;

            // verify the results
            b[7].refint = 777;
            for (int k = 0; k < N; ++k)
            {
                Console.WriteLine($"b[k]={b[k].refint}");
                Console.WriteLine($"acuda[k]={acuda[k].refint}");
                Console.WriteLine($"adotnet[k]={adotnet[k].refint}");
                Console.WriteLine();
            }

            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }
}