using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonitoringStarter.DataCollectorServiceReference;

namespace MonitoringStarter
{
    class Program
    {
        private static DataCollectorServiceClient _proxy;

        public static DataCollectorServiceClient Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new DataCollectorServiceClient();
                }

                return _proxy;
            }
            set { _proxy = value; }
        }

        static void Main(string[] args)
        {
            Config config = Config.LoadConfig();

            if (File.Exists(config.SystemManagerPath))
            {
                var processes = Process.GetProcessesByName("SystemManager");

                if (!processes.Any())
                {
                    var p1 = Process.Start(config.SystemManagerPath);
                }
            }
            Console.WriteLine("System Manager Started ...");

            Thread.Sleep(3 * 1000);

            if (File.Exists(config.ConsoleHostPath))
            {
                var processes = Process.GetProcessesByName("HostConsole");

                foreach (Process process in processes)
                {
                    process.Kill();
                }

                var p2 = Process.Start(config.ConsoleHostPath);
            }

            Console.WriteLine("Host Started ...");

            Thread.Sleep(3 * 1000);

            if (File.Exists(config.TelegramBotPath))
            {
                var processes = Process.GetProcessesByName("TelegramBot");

                foreach (Process process in processes)
                {
                    process.Kill();
                }

                var p3 = Process.Start(config.TelegramBotPath);
            }

            Console.WriteLine("Telegram Bot Started ...");

            Thread.Sleep(5*1000);

            Proxy.StartDataCollectorServer();

            Console.WriteLine("Collecting data ...");

            Console.ReadKey();
        }
    }
}
