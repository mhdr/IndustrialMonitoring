using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            Uri baseAddress1 = new Uri("http://172.20.63.234:9011/MonitoringService/DataCollectorService");
            Uri baseAddress2 = new Uri("http://172.20.63.234:9011/MonitoringService/NotificationService");
            Uri baseAddress3 = new Uri("http://172.20.63.234:9011/MonitoringService/ProcessDataService");
            Uri baseAddress4 = new Uri("http://172.20.63.234:9011/MonitoringService/UserService");

            Uri baseAddress5 = new Uri("net.tcp://172.20.63.234:9012/MonitoringService/DataCollectorService");
            Uri baseAddress6 = new Uri("net.tcp://172.20.63.234:9012/MonitoringService/NotificationService");
            Uri baseAddress7 = new Uri("net.tcp://172.20.63.234:9012/MonitoringService/ProcessDataService");
            Uri baseAddress8 = new Uri("net.tcp://172.20.63.234:9012/MonitoringService/UserService");

            ServiceHost host1 = new ServiceHost(typeof (DataCollectorService),
                baseAddress1);

            ServiceHost host2 = new ServiceHost(typeof(NotificationService),
    baseAddress2);

            ServiceHost host3 = new ServiceHost(typeof(ProcessDataService),
    baseAddress3);

            ServiceHost host4 = new ServiceHost(typeof(UserService),
    baseAddress4);

            ServiceHost host5 = new ServiceHost(typeof(DataCollectorService),
    baseAddress5);

            ServiceHost host6 = new ServiceHost(typeof(NotificationService),
    baseAddress6);

            ServiceHost host7 = new ServiceHost(typeof(ProcessDataService),
    baseAddress7);

            ServiceHost host8 = new ServiceHost(typeof(UserService),
    baseAddress8);

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

            ServiceMetadataBehavior smb5 = new ServiceMetadataBehavior();
            smb5.HttpGetEnabled = false;
            smb5.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host5.Description.Behaviors.Add(smb5);

            ServiceMetadataBehavior smb6 = new ServiceMetadataBehavior();
            smb6.HttpGetEnabled = false;
            smb6.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host6.Description.Behaviors.Add(smb6);

            ServiceMetadataBehavior smb7 = new ServiceMetadataBehavior();
            smb7.HttpGetEnabled = false;
            smb7.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host7.Description.Behaviors.Add(smb7);

            ServiceMetadataBehavior smb8 = new ServiceMetadataBehavior();
            smb8.HttpGetEnabled = false;
            smb8.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host8.Description.Behaviors.Add(smb8);

            host1.Open();
            host2.Open();
            host3.Open();
            host4.Open();

            host5.Open();
            host6.Open();
            host7.Open();
            host8.Open();




            Console.ReadKey();

            host1.Close();
            host2.Close();
            host3.Close();
            host4.Close();

            host5.Close();
            host6.Close();
            host7.Close();
            host8.Close();
        }


    }
}
