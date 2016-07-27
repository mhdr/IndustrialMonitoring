using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace MonitoringServiceLibrary
{
    public class NotificationsBotInvoker
    {
        private static NotificationsBotInvoker instance = null;
        private static readonly object padlock = new object();
        private Telegram.Bot.Api bot;

        private NotificationsBotInvoker()
        {
            bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");
        }

        public static void RegisterNewRecord(int notificationId, int notificationLogId)
        {
            try
            {
                IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

                var notification = entities.NotificationItems.FirstOrDefault(x => x.NotificationId == notificationId);
                var notificationLog =
                    entities.NotificationItemsLogs.FirstOrDefault(x => x.NotificationLogId == notificationLogId);

                if (notification == null)
                {
                    return;
                }

                if (notificationLog == null)
                {
                    return;
                }

                if (notification.DisableSendingNotificationInTelegram != null)
                {
                    if (notification.DisableSendingNotificationInTelegram.Value == true)
                    {
                        return;
                    }
                }

                if (
                    entities.UsersItemsPermissions.Any(
                        x => x.ItemId == notification.ItemId & x.Item.ReceiveDelayedNotificationInTelegram == false))
                {
                    NotificationBot notificationBot1 = new NotificationBot();
                    notificationBot1.NotificationId = notificationId;
                    notificationBot1.NotificationLogId = notificationLogId;
                    notificationBot1.WithoutAlarm = notificationLog.Value;
                    notificationBot1.RegisterTime = DateTime.Now;
                    notificationBot1.Delay = 0;
                    notificationBot1.IsSent = false;
                    notificationBot1.IsCompleted = false;

                    entities.NotificationBots.Add(notificationBot1);
                }


                if (
                    entities.UsersItemsPermissions.Any(
                        x => x.ItemId == notification.ItemId & x.Item.ReceiveDelayedNotificationInTelegram == true))
                {
                    if (notification.DelayForSendingNotificationInTelegram != null)
                    {
                        NotificationBot notificationBot2 = new NotificationBot();
                        notificationBot2.NotificationId = notificationId;
                        notificationBot2.NotificationLogId = notificationLogId;
                        notificationBot2.WithoutAlarm = notificationLog.Value;
                        notificationBot2.RegisterTime = DateTime.Now;
                        notificationBot2.Delay = notification.DelayForSendingNotificationInTelegram.Value;
                        notificationBot2.IsSent = false;
                        notificationBot2.IsCompleted = false;

                        entities.NotificationBots.Add(notificationBot2);
                    }
                }

                entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }
        }

        public static NotificationsBotInvoker Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NotificationsBotInvoker();
                    }
                    return instance;
                }
            }
        }

        public async Task<bool> SendNotification(int notificationLogId, NotificationDelayType delayType = NotificationDelayType.All)
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

                List<UsersItemsPermission> userPermissions = new List<UsersItemsPermission>();

                if (delayType == NotificationDelayType.All)
                {
                    userPermissions = entities.UsersItemsPermissions.Where(x => x.ItemId == itemId).ToList();
                }
                else if (delayType == NotificationDelayType.Normal)
                {
                    userPermissions = entities.UsersItemsPermissions.Where(x => x.ItemId == itemId & x.Item.ReceiveDelayedNotificationInTelegram == false).ToList();
                }
                else if (delayType == NotificationDelayType.Delayed)
                {
                    userPermissions = entities.UsersItemsPermissions.Where(x => x.ItemId == itemId & x.Item.ReceiveDelayedNotificationInTelegram == true).ToList();
                }


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
                    priority = (int)notificationItemsLog.NotificationItem.Priority;
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
    , notificationItemsLog.NotificationItem.NotificationMsg, emojiStatus, emojiRating, notificationItemsLog.Time);

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

        public async void CheckNotificationBot()
        {
            while (true)
            {
                IndustrialMonitoringEntities entities = null;
                List<NotificationBot> notificationBots = new List<NotificationBot>();

                try
                {
                    entities = new IndustrialMonitoringEntities();

                    notificationBots = entities.NotificationBots.Where(x => x.IsCompleted == false).OrderBy(x=>x.NotificationBotId).ToList();
                }
                catch (Exception ex)
                {
                    Logger.LogMonitoringServiceLibrary(ex);
                }

                foreach (NotificationBot n in notificationBots)
                {
                    try
                    {
                        if (n.Delay == 0)
                        {
                            await this.SendNotification(n.NotificationLogId, NotificationDelayType.Normal);
                            n.IsSent = true;
                            n.IsCompleted = true;
                            n.SentTime = DateTime.Now;

                            entities.SaveChanges();
                        }

                        bool timerElapsed = DateTime.Now - n.RegisterTime > TimeSpan.FromSeconds(n.Delay);

                        if (n.Delay > 0)
                        {
                            if (!n.NotificationItemsLog.Value)
                            {

                                // with alarm
                                // if still has alarm

                                if (timerElapsed)
                                {
                                    ItemsLogLatest itemLogLatest =
entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == n.NotificationItem.ItemId);

                                    double currentValue = double.Parse(itemLogLatest.Value);

                                    bool withoutNotification = false;

                                    if (n.NotificationItem.NotificationType == (int)NotificationType.Lower)
                                    {
                                        if (currentValue < n.NotificationItem.High)
                                        {
                                            withoutNotification = true;
                                        }
                                    }
                                    else if (n.NotificationItem.NotificationType == (int)NotificationType.Between)
                                    {
                                        if (currentValue > n.NotificationItem.Low & currentValue < n.NotificationItem.High)
                                        {
                                            withoutNotification = true;
                                        }
                                    }
                                    else if (n.NotificationItem.NotificationType == (int)NotificationType.Higher)
                                    {
                                        if (currentValue > n.NotificationItem.Low)
                                        {
                                            withoutNotification = true;
                                        }
                                    }

                                    if (!withoutNotification)
                                    {
                                        await this.SendNotification(n.NotificationLogId, NotificationDelayType.Delayed);

                                        n.IsSent = true;
                                        n.IsCompleted = true;
                                        n.SentTime = DateTime.Now;

                                        entities.SaveChanges();
                                    }
                                    else
                                    {
                                        n.IsSent = false;
                                        n.IsCompleted = true;

                                        entities.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                // without alarm

                                if (timerElapsed)
                                {
                                    IndustrialMonitoringEntities entities2 = new IndustrialMonitoringEntities();
                                    var lastNotificationBot =
                                        entities2.NotificationBots.Where(x => x.NotificationId == n.NotificationId & x.WithoutAlarm == false &
                                                                              x.Delay > 0 & x.IsCompleted == true).
                                            OrderByDescending(x => x.NotificationBotId).FirstOrDefault();

                                    if (lastNotificationBot == null)
                                    {
                                        continue;
                                    }

                                    if (lastNotificationBot.IsSent)
                                    {
                                        await this.SendNotification(n.NotificationLogId, NotificationDelayType.Delayed);

                                        n.IsSent = true;
                                        n.IsCompleted = true;
                                        n.SentTime = DateTime.Now;

                                        entities.SaveChanges();
                                    }
                                    else
                                    {
                                        n.IsSent = false;
                                        n.IsCompleted = true;

                                        entities.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogMonitoringServiceLibrary(ex);
                    }
                }

                await Task.Delay(1000);
            }
        }
    }
}
