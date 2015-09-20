using System;
using System.Collections.Generic;
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
            Console.ReadKey();
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
                    continue;
                }


                foreach (Update update in result)
                {
                    try
                    {
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
                                    await bot.SendTextMessage(update.Message.Chat.Id, "Your aren't authorized to access,please send your token.");
                                }

                                offset = update.Id + 1;
                                continue;
                            }

                            if (msg == "/list")
                            {
                                string output = "";

                                var items = entities.Items;

                                foreach (Item item in items)
                                {
                                    var category =
                                        entities.TabsItems.FirstOrDefault(x => x.ItemId == item.ItemId).Tab.TabName;

                                    string c = string.Format(@"Item Name : {0}
Item Id : {1}
Category : {2}

", item.ItemName, item.ItemId, category);

                                    output += c;
                                }

                                await bot.SendTextMessage(update.Message.Chat.Id, output);
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
                                                var category =
    entities.TabsItems.FirstOrDefault(x => x.ItemId == item.ItemId).Tab.TabName;

                                                string output = string.Format(@"Item Name : {0}
Item Id : {1}
Category : {2}
Value : {3}
Date : {4}", item.Item.ItemName, item.ItemId, category, item.Value, item.Time);
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
                            else if (msg == "alarms")
                            {

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
                                            var notifications = notificationService.GetNotificationLogs(1,
                                                DateTime.Now - new TimeSpan(hour, 0, 0),
                                                DateTime.Now).OrderBy(x => x.DateTime);

                                            string output = "";

                                            if (!notifications.Any())
                                            {
                                                await bot.SendTextMessage(update.Message.Chat.Id, "No Alarm");
                                            }

                                            foreach (NotificationLog notificationLog in notifications)
                                            {
                                                var category =
    entities.TabsItems.FirstOrDefault(x => x.ItemId == notificationLog.ItemId).Tab.TabName;

                                                string c = string.Format(@"Item : {0}
Item Id : {1}
Category : {2}
Description : {3}
Has Alarm : {4}
Date : {5}

", notificationLog.ItemName, notificationLog.ItemId, category,notificationLog.NotificationMsg, notificationLog.HasFault, notificationLog.DateTime);

                                                output += c;
                                            }

                                            if (!string.IsNullOrWhiteSpace(output))
                                            {
                                                await bot.SendTextMessage(update.Message.Chat.Id, output);
                                            }

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
