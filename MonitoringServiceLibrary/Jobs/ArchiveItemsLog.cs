using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using SharedLibrary;

namespace MonitoringServiceLibrary.Jobs
{
    public class ArchiveItemsLog:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime now = DateTime.Now;

                DateTime timeToArchive = now - new TimeSpan(0, 1, 0, 0);

                IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

                var items = entities.ItemsLogs.Where(x => x.Time < timeToArchive).ToList();
                List<ItemsLogArchive> allItems = new List<ItemsLogArchive>();

                foreach (ItemsLog item in items)
                {
                    ItemsLogArchive archive = new ItemsLogArchive();
                    archive.ItemId = item.ItemId;
                    archive.Time = item.Time;
                    archive.Value = item.Value;

                    allItems.Add(archive);
                }

                entities.ItemsLogArchives.AddRange(allItems);
                entities.ItemsLogs.RemoveRange(items);
                entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

        }
    }
}
