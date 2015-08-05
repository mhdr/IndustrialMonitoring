using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary;

namespace HostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress1 = new Uri("http://localhost:9011/MonitoringService/DataCollectorService");
            Uri baseAddress2 = new Uri("http://localhost:9011/MonitoringService/NotificationService");
            Uri baseAddress3 = new Uri("http://localhost:9011/MonitoringService/ProcessDataService");
            Uri baseAddress4 = new Uri("http://localhost:9011/MonitoringService/UserService");

            ServiceHost host1 = new ServiceHost(typeof (DataCollectorService),
                baseAddress1);

            ServiceHost host2 = new ServiceHost(typeof(NotificationService),
    baseAddress2);

            ServiceHost host3 = new ServiceHost(typeof(ProcessDataService),
    baseAddress3);

            ServiceHost host4 = new ServiceHost(typeof(UserService),
    baseAddress4);

            ServiceMetadataBehavior smb1 = new ServiceMetadataBehavior();
            smb1.HttpGetEnabled = true;
            smb1.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host1.Description.Behaviors.Add(smb1);

            ServiceMetadataBehavior smb2 = new ServiceMetadataBehavior();
            smb2.HttpGetEnabled = true;
            smb2.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host2.Description.Behaviors.Add(smb2);

            ServiceMetadataBehavior smb3 = new ServiceMetadataBehavior();
            smb3.HttpGetEnabled = true;
            smb3.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host3.Description.Behaviors.Add(smb3);

            ServiceMetadataBehavior smb4 = new ServiceMetadataBehavior();
            smb4.HttpGetEnabled = true;
            smb4.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host4.Description.Behaviors.Add(smb4);

            host1.Open();
            host2.Open();
            host3.Open();
            host4.Open();

            Console.ReadKey();

            host1.Close();
            host2.Close();
            host3.Close();
            host4.Close();
        }
    }
}
