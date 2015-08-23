using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using FillingServiceLibrary;

namespace FillingHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress1 = new Uri("http://192.168.10.5:9011/MonitoringService/ProcessDataService");

            ServiceHost host1 = new ServiceHost(typeof(ProcessDataService),
    baseAddress1);

            ServiceMetadataBehavior smb1 = new ServiceMetadataBehavior();
            smb1.HttpGetEnabled = true;
            smb1.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host1.Description.Behaviors.Add(smb1);

            host1.Open();

            Console.ReadKey();

            host1.Close();
        }
    }
}
