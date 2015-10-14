using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonitoringServiceLibrary;
using MonitoringServiceLibrary.ViewModels;
using SharedLibrary;
using Telegram.Bot.Types;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            StartResponseServer();
            StartLatestLogMonitor();
            Console.ReadKey();
        }

        public static async Task StartLatestLogMonitor()
        {
            IndustrialMonitoringEntities entities = null;
            string previousTimeStamp = "";
            var bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");

            while (true)
            {
                entities=new IndustrialMonitoringEntities();

                string timestamp = "";

                var items = entities.ItemsLogLatests;

                foreach (ItemsLogLatest item in items)
                {
                    timestamp += item.Time.ToBinary();
                }
                timestamp = Hash.GetHash(timestamp);

                var service =
                        entities.Services.FirstOrDefault(x => x.ServiceName == "RecieveLatesLogMonitorInTelegram");

                var users = entities.UsersServicesPermissions.Where(x => x.ServiceId == service.ServiceId);

                List<int> chatIds = new List<int>();

                foreach (UsersServicesPermission user in users)
                {
                    var userBot = entities.Bots.Where(x => x.UserId == user.UserId);

                    foreach (Bot bt in userBot)
                    {
                        if (bt.ChatId != null) chatIds.Add(bt.ChatId.Value);
                    }
                }

                if (previousTimeStamp == timestamp)
                {
                    foreach (int chatId in chatIds)
                    {
                        var emoji = "\u2734";
                        await bot.SendTextMessage(chatId, string.Format(@"{0} System Health {0}
Time : {1}",emoji,DateTime.Now));
                        await Task.Delay(100);
                    }
                }
                else
                {
                    foreach (int chatId in chatIds)
                    {
                        var emoji = "\u2733";
                        await bot.SendTextMessage(chatId, string.Format(@"{0} System Health {0}
Time : {1}", emoji, DateTime.Now));
                        await Task.Delay(100);
                    }
                }

                previousTimeStamp = timestamp;
                await Task.Delay(1*60*60*1000);
            }
        }

        public static async Task StartServer()
        {
            var bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");

            var offset = 0;
            Update[] result = null;
            IndustrialMonitoringEntities entities = null;

            while (true)
            {
                try
                {
                    entities = new IndustrialMonitoringEntities();
                    result = new Update[] { };

                    result = await bot.GetUpdates(offset);
                }
                catch (Exception ex)
                {
                    Logger.LogTelegramBot(ex);
                }

                if (result != null)
                {
                    foreach (Update u in result)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(u.Message.Text))
                            {
                                BotQueue botQueue = new BotQueue();
                                botQueue.ChatId = u.Message.Chat.Id;
                                botQueue.QueueDirection = (int)QueueDirection.In;
                                botQueue.Date = DateTime.Now;
                                botQueue.MessageText = u.Message.Text;
                                botQueue.IsCompleted = false;

                                entities.BotQueues.Add(botQueue);
                                entities.SaveChanges();

                                offset = u.Id + 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogTelegramBot(ex);
                        }

                    }
                }

                await Task.Delay(1000);
            }

        }

        public static async Task StartResponseServer()
        {
            var bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");

            var offset = 0;

            while (true)
            {
                Update[] result = new Update[] { };

                try
                {
                    result = await bot.GetUpdates(offset);
                }
                catch (Exception ex)
                {
                    Logger.LogTelegramBot(ex);
                    offset +=1;
                    continue;
                }


                foreach (Update update in result)
                {
                    try
                    {
                        if (update.Message.Type != MessageType.TextMessage)
                        {
                            offset = update.Id + 1;
                            continue;
                        }

                        if (update.Message.Text != null)
                        {
                            string msg = update.Message.Text.Trim().ToLower();

                            string log = string.Format("{0}", msg);
                            Console.WriteLine(log);

                            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

                            int chatId = update.Message.From.Id;

                            if (!entities.Bots.Any(x => x.ChatId == chatId))
                            {
                                var matched = entities.Bots.FirstOrDefault(x => x.Token == msg);

                                if (matched != null)
                                {
                                    matched.ChatId = chatId;
                                    matched.IsAuthorized = true;
                                    entities.SaveChanges();

                                    await bot.SendTextMessage(update.Message.Chat.Id, "Your token is saved");
                                }
                                else
                                {
                                    await bot.SendTextMessage(update.Message.Chat.Id, "You aren't authorized to access,please send your token.");
                                }

                                offset = update.Id + 1;
                                continue;
                            }

                            var user = entities.Bots.FirstOrDefault(x => x.ChatId == chatId).User;

                            if (msg == "/help")
                            {
                                string output = @"Examples :
/list : get list of items ( notice the itemId for items here )
/get all : get values for all items
/get 13 : get value for item with itemId13
/alarms 24h : get alarms in last 24 hours
/alarms 6h : get alarms in last 6 hours
";

                                await bot.SendTextMessage(chatId, output);
                            }
                            else if (msg == "/ping")
                            {
                                await bot.SendTextMessage(chatId, string.Format("{0} : Server is alive", DateTime.Now));
                            }
                            else if (msg == "/list")
                            {
                                var items = entities.UsersItemsPermissions.Where(x => x.UserId == user.UserId).OrderBy(x => x.Item.Order);

                                int i = 1;
                                int count = items.Count();

                                await bot.SendTextMessage(update.Message.Chat.Id, "** Start **");

                                foreach (var item in items)
                                {
                                    var category =
                                        entities.TabsItems.FirstOrDefault(x => x.ItemId == item.ItemId).Tab.TabName;

                                    string output = string.Format(@"Number : {0}/{1}
Item Name : {2}
Item Id : {3}
Category : {4}", i, count, item.Item.ItemName, item.ItemId, category);

                                    if (!string.IsNullOrEmpty(output))
                                    {
                                        await bot.SendTextMessage(update.Message.Chat.Id, output);
                                        await Task.Delay(10);
                                    }

                                    i++;
                                }

                                await bot.SendTextMessage(update.Message.Chat.Id, "** End **");
                            }
                            else if (msg == "/get all")
                            {
                                var items = entities.UsersItemsPermissions.Where(x => x.UserId == user.UserId).OrderBy(x => x.Item.Order);

                                int i = 1;
                                int count = items.Count();

                                await bot.SendTextMessage(update.Message.Chat.Id, "** Start **");

                                foreach (UsersItemsPermission item in items)
                                {
                                    var itemLog = entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == item.ItemId);
                                    var category = entities.TabsItems.FirstOrDefault(x => x.ItemId == item.ItemId).Tab.TabName;

                                    string unit = "";
                                    if (!string.IsNullOrEmpty(item.Item.Unit))
                                    {
                                        unit = item.Item.Unit;
                                    }

                                    string output = string.Format(@"Number : {0}/{1} 
Item Name : {2}
Item Id : {3}
Category : {4}
Value : {5} {6}
Date : {7}", i, count, item.Item.ItemName, item.ItemId, category, itemLog.Value, unit, itemLog.Time);

                                    if (!string.IsNullOrEmpty(output))
                                    {
                                        await bot.SendTextMessage(chatId, output);
                                        await Task.Delay(10);
                                    }

                                    i++;
                                }

                                await bot.SendTextMessage(update.Message.Chat.Id, "** End **");

                            }
                            else if (msg == "/alarms")
                            {
                                var allActiveNotifications = entities.NotificationItemsLogLatests.Where(x => x.Value == false);

                                var items = entities.UsersItemsPermissions.Where(x => x.UserId == user.UserId);

                                List<int> notificationIds = new List<int>();

                                foreach (UsersItemsPermission usersItemsPermission in items)
                                {
                                    var notifications = entities.NotificationItems.Where(x => x.ItemId == usersItemsPermission.ItemId);

                                    foreach (NotificationItem notification in notifications)
                                    {
                                        notificationIds.Add(notification.NotificationId);
                                    }
                                }

                                List<NotificationItemsLogLatest> result2=new List<NotificationItemsLogLatest>();
                                
                                foreach (NotificationItemsLogLatest notification in allActiveNotifications)
                                {
                                    if (notificationIds.Contains(notification.NotificationId))
                                    {
                                        result2.Add(notification);
                                    }
                                }

                                if (!result2.Any())
                                {
                                    await bot.SendTextMessage(update.Message.Chat.Id, "No active alarm");
                                    offset = update.Id + 1;
                                    continue;
                                }

                                await bot.SendTextMessage(update.Message.Chat.Id, "** Start **");

                                foreach (NotificationItemsLogLatest notification in result2)
                                {

                                    var category =
entities.TabsItems.FirstOrDefault(x => x.ItemId == notification.NotificationItem.ItemId).Tab.TabName;

                                    string emojiStatus = "";

                                    if (notification.Value)
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

                                    if (notification.NotificationItem.Priority != null)
                                    {
                                        priority = (int)notification.NotificationItem.Priority;
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
Date : {7}", emojiAlarm, notification.NotificationItem.Item.ItemName, notification.NotificationItem.ItemId, category
                        , notification.NotificationItem.NotificationMsg, emojiStatus, emojiRating, notification.Time);

                                    await bot.SendTextMessage(update.Message.Chat.Id, output);
                                    await Task.Delay(10);
                                }

                                await bot.SendTextMessage(update.Message.Chat.Id, "** End **");
                            }
                            else if (msg == "/alarms on")
                            {
                                var chat = entities.Bots.FirstOrDefault(x => x.ChatId == chatId);

                                if (chat != null)
                                {
                                    chat.ReceiveAlarms = true;
                                    entities.SaveChanges();

                                    await bot.SendTextMessage(chatId, "You will receive alarms from now on");
                                }
                            }
                            else if (msg == "/alarms off")
                            {
                                var chat = entities.Bots.FirstOrDefault(x => x.ChatId == chatId);

                                if (chat != null)
                                {
                                    chat.ReceiveAlarms = false;
                                    entities.SaveChanges();

                                    await bot.SendTextMessage(chatId, "You won't receive alarms from now on");
                                }
                            }
                            else if (msg.StartsWith("/get "))
                            {
                                var parts = msg.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                if (parts.Count() == 2)
                                {
                                    int id;

                                    if (int.TryParse(parts[1], out id))
                                    {
                                        if (entities.Items.Any(x => x.ItemId == id))
                                        {
                                            var item = entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == id);

                                            if (item != null)
                                            {
                                                if (!
                                                    entities.UsersItemsPermissions.Any(
                                                        x => x.UserId == user.UserId && x.ItemId == item.ItemId))
                                                {
                                                    await bot.SendTextMessage(chatId, "You don't have access to this item");
                                                    offset = update.Id + 1;
                                                    continue;
                                                }

                                                string unit = "";
                                                if (!string.IsNullOrEmpty(item.Item.Unit))
                                                {
                                                    unit = item.Item.Unit;
                                                }

                                                var category =
    entities.TabsItems.FirstOrDefault(x => x.ItemId == item.ItemId).Tab.TabName;

                                                string output = string.Format(@"Item Name : {0}
Item Id : {1}
Category : {2}
Value : {3} {4}
Date : {5}", item.Item.ItemName, item.ItemId, category, item.Value, unit, item.Time);
                                                await bot.SendTextMessage(update.Message.Chat.Id, output);
                                            }
                                        }
                                        else
                                        {
                                            await bot.SendTextMessage(update.Message.Chat.Id, "Invalid Id");
                                        }
                                    }
                                }
                            }
                            else if (msg.StartsWith("/alarms "))
                            {
                                var parts = msg.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                if (parts.Count() == 2)
                                {
                                    var part2 = parts[1];

                                    var index = part2.IndexOf("h");

                                    if (index > 0)
                                    {
                                        var hourStr = part2.Substring(0, index);
                                        int hour;

                                        if (int.TryParse(hourStr, out hour))
                                        {
                                            NotificationService notificationService = new NotificationService();
                                            var notifications = notificationService.GetNotificationLogs(user.UserId,
                                                DateTime.Now - new TimeSpan(hour, 0, 0),
                                                DateTime.Now).OrderBy(x => x.DateTime);


                                            if (!notifications.Any())
                                            {
                                                await bot.SendTextMessage(update.Message.Chat.Id, "No Alarm");
                                                offset = update.Id + 1;
                                                continue;
                                            }

                                            await bot.SendTextMessage(update.Message.Chat.Id, "** Start **");

                                            int i = 1;
                                            int count = notifications.Count();

                                            foreach (NotificationLog notificationLog in notifications)
                                            {
                                                var category =
    entities.TabsItems.FirstOrDefault(x => x.ItemId == notificationLog.ItemId).Tab.TabName;

                                                string output = string.Format(@"Alarm :
Number : {0}/{1}
Item : {2}
Item Id : {3}
Category : {4}
Description : {5}
Has Alarm : {6}
Date : {7}", i, count, notificationLog.ItemName, notificationLog.ItemId, category, notificationLog.NotificationMsg, notificationLog.HasFault, notificationLog.DateTime);

                                                i++;
                                                if (!string.IsNullOrWhiteSpace(output))
                                                {
                                                    await bot.SendTextMessage(update.Message.Chat.Id, output);
                                                    await Task.Delay(10);
                                                }
                                            }

                                            await bot.SendTextMessage(update.Message.Chat.Id, "** End **");

                                        }
                                    }
                                }
                            }
                            else
                            {
                                await bot.SendTextMessage(update.Message.Chat.Id, "Wrong command");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogTelegramBot(ex);
                    }
                    finally
                    {
                        offset = update.Id + 1;
                    }
                }

                await Task.Delay(1000);
            }
        }
    }
}
