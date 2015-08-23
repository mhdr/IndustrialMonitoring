using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.ProcessDataServiceClient processDataServiceClient =
                new ServiceReference1.ProcessDataServiceClient();

            Console.WriteLine(processDataServiceClient.GetSterilizerZoneTemperature());

            Console.ReadKey();
        }
    }
}
