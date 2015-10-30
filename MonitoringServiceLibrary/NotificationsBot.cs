using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace MonitoringServiceLibrary
{
    public class NotificationsBot
    {
        private static NotificationsBot instance = null;
        private static readonly object padlock = new object();
        private Telegram.Bot.Api bot;

        private NotificationsBot()
        {
            bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");
        }

        public static NotificationsBot Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NotificationsBot();
                    }
                    return instance;
                }
            }
        }

        public async Task HandleSendNotification(int notificationLogId)
        {
            try
            {
                int i = 0;
                var result = false;
                int delay = 1000;

                while (!result)
                {
                    if (i > 5)
                    {
                        return;
                    }

                    result = await SendNotification(notificationLogId);

                    await Task.Delay(delay);

                    delay += 1000;
                    i++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }
        }

        public async Task<bool> SendNotification(int notificationLogId,NotificationDelayType delayType=NotificationDelayType.All)
        {
            try
            {
                IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

                var notificationItemsLog =
                    entities.NotificationItemsLogs.FirstOrDefault(x => x.NotificationLogId == notificationLogId);

                if (notificationItemsLog == null)
                {
                    return false;
                }

                int itemId = notificationItemsLog.NotificationItem.ItemId;

                List<UsersItemsPermission> userPermissions=new List<UsersItemsPermission>();

                userPermissions = entities.UsersItemsPermissions.Where(x => x.ItemId == itemId).ToList();

                List<int> chatIds = new List<int>();

                foreach (UsersItemsPermission usersItemsPermission in userPermissions)
                {
                    int userId = usersItemsPermission.UserId;

                    var ids = entities.Bots.Where(x => x.UserId == userId & x.ReceiveAlarms == true);

                    if (!ids.Any())
                    {
                        continue;
                    }

                    foreach (Bot id in ids)
                    {
                        if (id.ChatId != null)
                        {
                            if (id.ChatId > 0)
                            {
                                chatIds.Add((int)id.ChatId);
                            }
                        }

                    }
                }

                var category =
    entities.TabsItems.FirstOrDefault(x => x.ItemId == notificationItemsLog.NotificationItem.ItemId).Tab.TabName;

                string emojiStatus = "";

                if (notificationItemsLog.Value)
                {
                    emojiStatus = "\u2705";
                }
                else
                {
                    emojiStatus = "\u274C";
                }

                string emojiAlarm = "\u2757";

                string emojiRating = "";

                int priority = 0;

                if (notificationItemsLog.NotificationItem.Priority != null)
                {
                    priority = (int) notificationItemsLog.NotificationItem.Priority;
                }

                if (priority > 0)
                {
                    for (int i = 1; i <= priority; i++)
                    {
                        emojiRating += "\u2B50";
                    }    
                }
                
                string output = string.Format(@"{0} Alarm {0}
Item Name : {1}
Item Id : {2}
Category : {3}
Description : {4}
Status : {5}
Priority : {6}
Date : {7}", emojiAlarm, notificationItemsLog.NotificationItem.Item.ItemName, notificationItemsLog.NotificationItem.ItemId, category
    , notificationItemsLog.NotificationItem.NotificationMsg, emojiStatus,emojiRating, notificationItemsLog.Time);

                foreach (int chatId in chatIds)
                {
                    await bot.SendTextMessage(chatId, output);
                    await Task.Delay(100);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

            return false;
        }
    }
}
