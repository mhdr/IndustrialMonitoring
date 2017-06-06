using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace MonitoringServiceLibrary.Jobs
{
    public class OpcBooleanCommandJob:IJob
    {
        IndustrialMonitoringEntities entities=new IndustrialMonitoringEntities();

        public void Execute(IJobExecutionContext context)
        {
            
        }
    }
}
