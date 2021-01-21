using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Uruchomienie sekwencyjne");

            Watek1();
            Watek2("    ");

            for(int i = 0; i < 10; i++)
                Console.WriteLine(i);

            // ====================

            Console.WriteLine("Uruchomienie wielowątkowe");
            //Thread t1 = new Thread(new ThreadStart(Watek1));
            Thread t1 = new Thread( Watek1 );
            t1.Start();

            //Thread t2
            //Thread t2 = new Thread( () => Watek2("    ") );
            //t2.Start();
            Thread t2 = new Thread(Watek2);
            t2.IsBackground = true;
            t2.Start("    ");

            for (int i = 0; i < 10; i++)
                Console.WriteLine(i);

            t2.Join();
            Console.WriteLine("Wszystkie wątki zakończone normalnie");
        }


        static void Watek1()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("  " + i);
                Thread.Sleep(100);
            }
        }

        static void Watek2( object prefix )
        {
            for (int i = 64; i < 64 + 10; i++)
            {
                Console.WriteLine(prefix.ToString() + (char)i);
                Thread.Sleep(200);
            }
        }

    }
}
