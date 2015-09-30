using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedLibrary;

namespace NetworkInterfaceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface n in interfaces)
            {
                var json = JsonConvert.SerializeObject(n, Formatting.Indented);
                Console.WriteLine(json);

                Logger.LogNetworkInterfaces(json);
            }

            Console.ReadKey();
        }
    }
}
