using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            string www1 = "http://wsei.edu.pl";
            string www2 = "http://wp.pl";

            int n1 = CountCharsInPage(www1);
            int n2 = CountCharsInPage(www2);
            Console.WriteLine($"{n1} , {n2}");

            // ===== async
            n1 = n2 = 0;
            Task<int> t1 = CountCharsInPageAsync(www1);
            Task<int> t2 = CountCharsInPageAsync(www2);
            Task.WaitAny();           

            n1 = t1.Result;
            n2 = t2.Result;
            Console.WriteLine($"{n1} , {n2}");
        }

        static int CountCharsInPage( string uri )
        {
            WebClient wc = new WebClient();

            string page = wc.DownloadString(uri);
            return page.Length;
        }

        static async Task<int> CountCharsInPageAsync(string uri)
        {
            WebClient wc = new WebClient();

            string page = await wc.DownloadStringTaskAsync( new Uri(uri) );
            return page.Length;
        }

    }
}
