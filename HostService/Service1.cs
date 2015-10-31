using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HostService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Config config = Config.LoadConfig();

            if (File.Exists(config.Path))
            {
                var processes = Process.GetProcessesByName("MonitoringStarter");

                foreach (Process process in processes)
                {
                    process.Kill();
                }

                var p1 = Process.Start(config.Path);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
