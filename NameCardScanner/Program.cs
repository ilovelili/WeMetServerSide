using Nancy.Hosting.Self;
using System;
using System.Diagnostics;

namespace NamecardScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            var nancyHost = new NancyHost(new Uri("http://localhost:8888"), new Uri("http://127.0.0.1:8888"));
            nancyHost.Start();

            Console.WriteLine("Nancy now listening - navigating to http://localhost:8888. Press enter to stop");
            Process.Start("http://localhost:8888");
            Console.ReadKey();
            nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }        
    }    
}
