using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace MonitoringServiceLibrary.Jobs
{
    public class ArchiveItemsLog:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            DateTime now=DateTime.Now;

            DateTime lastWeek = now - new TimeSpan(0, 12, 0, 0);

            IndustrialMonitoringEntities entities=new IndustrialMonitoringEntities();

            var items= entities.ItemsLogs.Where(x => x.Time < lastWeek);

            foreach (ItemsLog item in items)
            {
                ItemsLogArchive archive=new ItemsLogArchive();
                archive.ItemId = item.ItemId;
                archive.Time = item.Time;
                archive.Value = item.Value;
                entities.SaveChanges();

                entities.ItemsLogs.Remove(item);
                entities.SaveChanges();
            }
        }
    }
}
