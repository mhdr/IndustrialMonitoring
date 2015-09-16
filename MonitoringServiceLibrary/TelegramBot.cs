using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonitoringServiceLibrary.ViewModels;
using Telegram.Bot.Types;

namespace MonitoringServiceLibrary
{
    public class TelegramBot
    {
        private static TelegramBot instance=null;
        private static readonly object padlock = new object();

        public static TelegramBot Initialize()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance=new TelegramBot();
                }

                return instance;
            }
        }
        
        private TelegramBot()
        {
        }

        public void StartResponseServer()
        {
            try
            {
                var bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");

                var offset = 0;

                Thread thread = new Thread(async () =>
                {

                    while (true)
                    {
                        var result = await bot.GetUpdates(offset);

                        foreach (Update update in result)
                        {
                            if (update.Message.Text != null)
                            {
                                string msg = update.Message.Text.Trim().ToLower();

                                if (msg == "list")
                                {
                                    var entities = new IndustrialMonitoringEntities();

                                    string output = "";

                                    var items = entities.Items;

                                    foreach (Item item in items)
                                    {
                                        string c = string.Format(@"Item Name : {0}
Item Id : {1}

", item.ItemName, item.ItemId);

                                        output += c;
                                    }

                                    await bot.SendTextMessage(update.Message.Chat.Id, output);
                                }
                                else if (msg.StartsWith("get "))
                                {
                                    var parts = msg.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                    if (parts.Count() == 2)
                                    {
                                        int id;

                                        if (int.TryParse(parts[1], out id))
                                        {
                                            var entities = new IndustrialMonitoringEntities();
                                            if (entities.Items.Any(x => x.ItemId == id))
                                            {
                                                var item = entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == id);

                                                if (item != null)
                                                {
                                                    string output = string.Format(@"Item Name : {0}
Value : {1}
Date : {2}", item.Item.ItemName, item.Value, item.Time);
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
                                else if (msg.StartsWith("alarms"))
                                {
                                    var parts = msg.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);

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
                                                var notifications = notificationService.GetNotificationLogs(1, DateTime.Now - new TimeSpan(hour, 0, 0),
                                                    DateTime.Now).OrderBy(x=>x.DateTime);

                                                string output = "";
                                                foreach (NotificationLog notificationLog in notifications)
                                                {
                                                    string c = string.Format(@"Item : {0}
Description : {1}
Date : {2}
Has Alarm : {3}

", notificationLog.ItemName, notificationLog.NotificationMsg, notificationLog.DateTime.ToString(), notificationLog.HasFault.ToString());

                                                    output += c;
                                                }

                                                await bot.SendTextMessage(update.Message.Chat.Id, output);
                                            }
                                        }
                                    }
                                }
                            }

                            offset = update.Id + 1;
                        }

                        await Task.Delay(1000);
                    }
                });


                thread.Start();
            }
            catch (Exception ex)
            {
                TelegramBot.Initialize().StartResponseServer();
            }
            
        }
    }
}
