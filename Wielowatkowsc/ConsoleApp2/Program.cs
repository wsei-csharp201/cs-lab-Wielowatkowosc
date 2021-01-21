using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        static int[] a;
        static int counter = 0;

        private static readonly object blokada = new object();
        private volatile static int max = int.MinValue;

        static void Main(string[] args)
        {
            a = genTab(10_000_000);
            Proc1SequentialFullArray();
            Proc2SequentialTwoArray();
            Proc3MultiThreadTwoArray();
            Proc4MultiThreadTwoArraySharedMax();
            Proc5MultiThreadTwoArraySharedMaxLock();
            Proc5MultiTask();
            Proc6ParallelTwoArray();
            Proc7ParallelFullArray();

            IncrementDecrement();
            IncrementDecrementMultiThread();
            IncrementDecrementMultiThreadLockShared();
        }


        static void Proc1SequentialFullArray()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int max = Max(a, 0, a.Length - 1);
            Console.WriteLine($"max = {max} calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc2SequentialTwoArray()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int max1 = Max(a, 0, a.Length / 3);
            int max2 = Max(a, a.Length / 3 + 1, a.Length - 1);
            Console.WriteLine($"max = { ((max1 > max2) ? max1 : max2) } calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc3MultiThreadTwoArray()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int max1 = 0;
            Thread t1 = new Thread(() => { max1 = Max(a, 0, a.Length / 3); });

            int max2 = 0;
            Thread t2 = new Thread(() => { max2 = Max(a, a.Length / 3 + 1, a.Length - 1); });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine($"max = { ((max1 > max2) ? max1 : max2) } calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc4MultiThreadTwoArraySharedMax()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Thread t1 = new Thread(() => { MaxShared(a, 0, a.Length / 3); });

            Thread t2 = new Thread(() => { MaxShared(a, a.Length / 3 + 1, a.Length - 1); });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine($"max = { max } calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc5MultiThreadTwoArraySharedMaxLock()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Thread t1 = new Thread(() => { MaxSharedLock(a, 0, a.Length / 3); });

            Thread t2 = new Thread(() => { MaxSharedLock(a, a.Length / 3 + 1, a.Length - 1); });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine($"max = { max } calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc5MultiTask()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Task<int> tm1 = MaxAsync(a, 0, a.Length / 3);
            Task<int> tm2 = MaxAsync(a, a.Length / 3 + 1, a.Length - 1);
            //tm1.Wait();
            //tm2.Wait();
            Task.WaitAll();
            int max1 = tm1.Result;
            int max2 = tm2.Result;

            Console.WriteLine($"max = { ((max1 > max2) ? max1 : max2) } calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc6ParallelTwoArray()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int max1 = 0;
            int max2 = 0;
            Parallel.Invoke(
                                () => { max1 = Max(a, 0, a.Length / 3); },
                                () => { max2 = Max(a, a.Length / 3 + 1, a.Length - 1); }
                            );
            Console.WriteLine($"max = { ((max1 > max2) ? max1 : max2) } calculated in {sw.ElapsedMilliseconds} ms");
        }

        static void Proc7ParallelFullArray()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int max = MaxParallelFor(a, 0, a.Length - 1);
            Console.WriteLine($"max = {max} calculated in {sw.ElapsedMilliseconds} ms");
        }

        static int MaxParallelFor(int[] a, int left, int right)
        {
            if (left > right || right > a.Length)
                throw new ArgumentException();

            int m = int.MinValue;

            Parallel.For(left, right, i => { if (m < a[i]) m = a[i]; }  );

            return m;
        }

        static int Max(int[] a, int left, int right)
        {
            if (left > right || right > a.Length)
                throw new ArgumentException();

            int m = int.MinValue;
            for (int i = left; i <= right; i++)
            {
                if (m < a[i])
                    m = a[i];
            }
            return m;
        }

        static void MaxShared(int[] a, int left, int right)
        {
            if (left > right || right > a.Length)
                throw new ArgumentException();

            for (int i = left; i <= right; i++)
            {
                if (max < a[i])
                    max = a[i];
            }
        }

        static void MaxSharedLock(int[] a, int left, int right)
        {
            if (left > right || right > a.Length)
                throw new ArgumentException();

            for (int i = left; i <= right; i++)
            {
                lock (blokada)
                {
                    if (max < a[i])
                        max = a[i];
                }
            }
        }


        static async Task<int> MaxAsync(int[] a, int left, int right)
        {
            int m = await Task.Run( () => Max(a, left, right) );
            return m;
        }


        static int[] genTab(int size)
        {
            int[] tab = new int[size];
            Random los = new Random();

            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = los.Next(int.MinValue, int.MaxValue);
            }

            return tab;
        }

        static void IncrementDecrement()
        {
            for (int i = 0; i < 10_000; i++)
                counter++;
            for (int i = 0; i < 10_000; i++)
                counter--;
            Console.WriteLine("counter = " + counter);
        }





        static void IncrementDecrementMultiThread()
        {
            Thread t1 = new Thread(
              () =>
                {
                    for (int i = 0; i < 1_000_000; i++)
                        counter++;
                }
            );

            Thread t2 = new Thread(
              () =>
              {
                  for (int i = 0; i < 1_000_000; i++)
                      counter--;
              }
            );
            t1.Start(); t2.Start();

            t1.Join(); t2.Join();

            Console.WriteLine("counter = " + counter);
        }

        static void IncrementDecrementMultiThreadLockShared()
        {
            Thread t1 = new Thread(
              () =>
              {
                  for (int i = 0; i < 1_000_000; i++)
                      lock ( blokada )
                      {
                          counter++;
                      }
              }
            );

            Thread t2 = new Thread(
              () =>
              {
                  for (int i = 0; i < 1_000_000; i++)
                      lock ( blokada )
                      {
                          counter--;
                      }
              }
            );
            t1.Start(); t2.Start();

            t1.Join(); t2.Join();

            Console.WriteLine("counter = " + counter);
        }

    }
}
