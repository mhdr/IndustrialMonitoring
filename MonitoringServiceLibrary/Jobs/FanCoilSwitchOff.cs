using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace MonitoringServiceLibrary.Jobs
{
    public class FanCoilSwitchOff:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            TechnicalFanCoil technicalFanCoil=new TechnicalFanCoil();
            technicalFanCoil.TurnOffMotor1();
            technicalFanCoil.TurnOffMotor2();

            var bot = new Telegram.Bot.Api("208761880:AAHxUZCc5z0g2dJDrgbbMEWk7r1t_IoKiAw");

            var chatIds = RecieveReportForTechnicalFanCoilChatIds();
            string report = "Status automatically changed to off.";

            foreach (var id in chatIds)
            {
                bot.SendTextMessage(id, report);
                bot.SendTextMessage(id, technicalFanCoil.GetStatus());
            }
        }

        public static List<int> RecieveReportForTechnicalFanCoilChatIds()
        {
            var entities = new IndustrialMonitoringEntities();
            var service =
        entities.Services.FirstOrDefault(x => x.ServiceName == "RecieveReportForTechnicalFanCoil");

            var users = entities.UsersServicesPermissions.Where(x => x.ServiceId == service.ServiceId);

            List<int> chatIds = new List<int>();

            foreach (UsersServicesPermission u in users)
            {
                var userBot = entities.FanCoilBots.Where(x => x.UserId == u.UserId);

                foreach (var bt in userBot)
                {
                    if (bt.ChatId != null) chatIds.Add(bt.ChatId.Value);
                }
            }

            return chatIds;
        }
    }
}
