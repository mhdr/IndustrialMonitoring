using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonitoringServiceLibrary;
using MonitoringServiceLibrary.Jobs;
using MonitoringServiceLibrary.ViewModels;
using Quartz;
using Quartz.Impl;
using SharedLibrary;
using SharedLibrarySocket;
using SharedLibrarySocket.Warpper;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = MonitoringServiceLibrary.User;

namespace TelegramBot
{
    class Program
    {
        private static int FanCoilPort = 4200;
        static void Main(string[] args)
        {
            StartResponseServer();
            StartLatestLogMonitor();
            StartTechnicalFanCoilBot();
            SatrtQuartzScheduler();
            SatrtQuartzSchedulerForArchive();

            Thread thread=new Thread(()=>StartFanCoilMobileServer());
            thread.Start();

            //Thread thread2 = new Thread(() =>
            //  {
            //      StartEchoServer();
            //  });
            //thread2.Start();

            Console.ReadKey();
        }

        public static void StartEchoServer()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 4210);

            socket.Bind(endPoint);
            socket.Listen(10);

            while (true)
            {
                try
                {
                    Console.WriteLine("waiting for new connection...");

                    Socket newSocket = socket.Accept();
                    ThreadPool.QueueUserWorkItem((state => OnNewSocketAccept(newSocket)));
                }
                catch (Exception ex)
                {
                    Logger.LogTelegramBot(ex);
                }

            }
        }

        public static async Task SatrtQuartzScheduler()
        {
            try
            {
                // construct a scheduler factory
                ISchedulerFactory schedFact = new StdSchedulerFactory();

                // get a scheduler
                IScheduler sched = schedFact.GetScheduler();
                sched.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<FanCoilSwitchOff>()
                    .WithIdentity("SwitchOffFanCoil", "FanCoil")
                    .Build();

                DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);

                // Trigger the job to run now, and then every 24 hours
                ITrigger trigger = TriggerBuilder.Create()
                  .WithIdentity("Trigger1", "group1")
                  .StartAt(new DateTimeOffset(startTime))
                  .WithSimpleSchedule(x => x
                      .WithIntervalInHours(24)
                      .RepeatForever())
                  .Build();

                sched.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                Logger.LogTelegramBot(ex);
            }

        }

        public static async Task SatrtQuartzSchedulerForArchive()
        {
            try
            {
                // construct a scheduler factory
                ISchedulerFactory schedFact = new StdSchedulerFactory();

                // get a scheduler
                IScheduler sched = schedFact.GetScheduler();
                sched.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<ArchiveItemsLog>()
                    .WithIdentity("ArchiveItems", "Items")
                    .Build();

                // Trigger the job to run now, and then every 1 hours
                ITrigger trigger = TriggerBuilder.Create()
                  .WithIdentity("Trigger2", "group2")
                  .StartNow()
                  .WithSimpleSchedule(x => x
                      .WithIntervalInHours(1)
                      .RepeatForever())
                  .Build();

                sched.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                Logger.LogTelegramBot(ex);
            }
        }

        public static async Task StartLatestLogMonitor()
        {
            IndustrialMonitoringEntities entities = null;
            string previousTimeStamp = "";
            var bot = new Telegram.Bot.Api("133038323:AAFXVhA9Htj3p0a0Sl3hydt65Y7fl2AOVEI");
            DateTime lastPeriodicSendTime = DateTime.MinValue;
            bool earlyAlertSent = false;

            while (true)
            {
                try
                {
                    entities = new IndustrialMonitoringEntities();

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

                    PerformanceCounter cpuCounter=new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    PerformanceCounter ramCounter= new PerformanceCounter("Memory", "Available MBytes");

                    // The method nextValue() always returns a 0 value on the first call. So you have to call this method a second time
                    cpuCounter.NextValue();
                    Thread.Sleep(1000);
                    var cpuUsage = cpuCounter.NextValue();
                    string cpuUsageStr = string.Format("{0:f2} %",cpuUsage);

                    var ramAvailable = ramCounter.NextValue();
                    string ramAvaiableStr = string.Format("{0} MB", ramAvailable);

                    if (previousTimeStamp == timestamp)
                    {
                        // system has error

                        if (DateTime.Now - lastPeriodicSendTime > TimeSpan.FromHours(1))
                        {
                            foreach (int chatId in chatIds)
                            {
                                var emoji = "\u2734";
                                await bot.SendTextMessage(chatId, string.Format(@"{0} System Health {0}
Time : {1}
CPU Usage : {2}
Memory Available : {3}", emoji, DateTime.Now,cpuUsageStr,ramAvaiableStr));
                                await Task.Delay(100);
                            }

                            lastPeriodicSendTime = DateTime.Now;

                            Process.Start("shutdown.exe", "-f -r -t 0");
                        }
                        else
                        {
                            if (!earlyAlertSent)
                            {
                                foreach (int chatId in chatIds)
                                {
                                    var emoji = "\u2734";
                                    await bot.SendTextMessage(chatId, string.Format(@"{0} System Health {0}
Time : {1}
CPU Usage : {2}
Memory Available : {3}", emoji, DateTime.Now, cpuUsageStr, ramAvaiableStr));
                                    await Task.Delay(100);
                                }

                                earlyAlertSent = true;
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Now - lastPeriodicSendTime > TimeSpan.FromHours(1))
                        {
                            foreach (int chatId in chatIds)
                            {
                                var emoji = "\u2733";
                                await bot.SendTextMessage(chatId, string.Format(@"{0} System Health {0}
Time : {1}
CPU Usage : {2}
Memory Available : {3}", emoji, DateTime.Now, cpuUsageStr, ramAvaiableStr));
                                await Task.Delay(100);
                            }

                            // reset early alert
                            earlyAlertSent = false;

                            lastPeriodicSendTime = DateTime.Now;
                        }
                    }

                    previousTimeStamp = timestamp;
                    await Task.Delay(2 * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Logger.LogTelegramBot(ex);
                }
            }
        }

        public static async Task StartTechnicalFanCoilBot()
        {
            var bot = new Telegram.Bot.Api("208761880:AAHxUZCc5z0g2dJDrgbbMEWk7r1t_IoKiAw");

            var offset = 0;
            Dictionary<int,int> wizardStep=new Dictionary<int, int>();

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
                    offset += 1;
                    continue;
                }

                foreach (Update update in result)
                {
                    try
                    {
                        if (update.Message.Text != null)
                        {
                            string msg = update.Message.Text.Trim().ToLower();
                            int chatId = update.Message.From.Id;
                            int msgId = update.Message.MessageId;
                            User currentUser = GetUserInTechnicalFanCoil(chatId);

                            string log = "";
                            if (currentUser != null)
                            {
                                log = string.Format("{0} {1} : {2}", currentUser.FirstName, currentUser.LastName, msg);
                            }
                            else
                            {
                                log = string.Format("Anonymous : {0}", msg);
                            }
                            
                            Console.WriteLine(log);

                            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

                            if (!entities.FanCoilBots.Any(x => x.ChatId == chatId))
                            {
                                var matched = entities.FanCoilBots.FirstOrDefault(x => x.Token == msg);

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

                            TechnicalFanCoil technicalFanCoil=new TechnicalFanCoil();

                            if (wizardStep.ContainsKey(chatId))
                            {
                                int step = wizardStep[chatId];
                                bool isCancled=false;

                                if (step == 11)
                                {
                                    if (msg == "Off".ToLower())
                                    {
                                        technicalFanCoil.TurnOffMotor1();
                                    }
                                    else if (msg == "Speed 1".ToLower())
                                    {
                                        technicalFanCoil.ChangeSpeedMotor1(1);
                                    }
                                    else if (msg == "Speed 2".ToLower())
                                    {
                                        technicalFanCoil.ChangeSpeedMotor1(2);
                                    }
                                    else if (msg == "Speed 3".ToLower())
                                    {
                                        technicalFanCoil.ChangeSpeedMotor1(3);
                                    }
                                    else
                                    {
                                        isCancled = true;
                                    }

                                    wizardStep.Remove(chatId);
                                }
                                else if (step == 21)
                                {
                                    if (msg == "Off".ToLower())
                                    {
                                        technicalFanCoil.TurnOffMotor2();
                                    }
                                    else if (msg == "Speed 1".ToLower())
                                    {
                                        technicalFanCoil.ChangeSpeedMotor2(1);
                                    }
                                    else if (msg == "Speed 2".ToLower())
                                    {
                                        technicalFanCoil.ChangeSpeedMotor2(2);
                                    }
                                    else if (msg == "Speed 3".ToLower())
                                    {
                                        technicalFanCoil.ChangeSpeedMotor2(3);
                                    }
                                    else
                                    {
                                        isCancled = true;
                                    }

                                    wizardStep.Remove(chatId);
                                }

                                if (!isCancled)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }
                                }

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;
                                await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                            }
                            else if (msg == "/fan_coil_off")
                            {
                                var result1=technicalFanCoil.TurnOffMotor1();
                                var result2=technicalFanCoil.TurnOffMotor2();

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1 == true && result2 == true)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                            }
                            else if (msg== "/motor1_off")
                            {
                                var result1=technicalFanCoil.TurnOffMotor1();

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                            }
                            else if (msg == "/motor2_off")
                            {
                                var result1=technicalFanCoil.TurnOffMotor2();

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                            }
                            else if (msg == "/motor1_speed1")
                            {
                                var result1=technicalFanCoil.ChangeSpeedMotor1(1);

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                            }
                            else if (msg == "/motor1_speed2")
                            {
                                var result1=technicalFanCoil.ChangeSpeedMotor1(2);

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                            }
                            else if (msg == "/motor1_speed3")
                            {
                                var result1=technicalFanCoil.ChangeSpeedMotor1(3);

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                                
                            }
                            else if (msg == "/motor2_speed1")
                            {
                                var result1=technicalFanCoil.ChangeSpeedMotor2(1);

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                                
                            }
                            else if (msg == "/motor2_speed2")
                            {
                                var result1=technicalFanCoil.ChangeSpeedMotor2(2);

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                                
                            }
                            else if (msg == "/motor2_speed3")
                            {
                                var result1=technicalFanCoil.ChangeSpeedMotor2(3);

                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;

                                if (result1)
                                {
                                    var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                                    string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                                    foreach (var id in chatIds)
                                    {
                                        if (id != chatId)
                                        {
                                            await bot.SendTextMessage(id, report);
                                            await bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                                        }
                                    }

                                    await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "Failed.", false, false, 0, replyKeyboardHide);
                                }
                                
                            }
                            else if (msg == "/status")
                            {
                                ReplyKeyboardHide replyKeyboardHide = new ReplyKeyboardHide();
                                replyKeyboardHide.HideKeyboard = true;
                                await bot.SendTextMessage(chatId, technicalFanCoil.GetStatus(), false, false, 0, replyKeyboardHide);
                            }
                            else if (msg == "/motor1")
                            {
                                ReplyKeyboardMarkup replyKeyboardMarkup=new ReplyKeyboardMarkup();
                                replyKeyboardMarkup.OneTimeKeyboard = true;
                                replyKeyboardMarkup.Keyboard=new KeyboardButton[][]
                                {
                                    new KeyboardButton[] {"Off"},
                                    new KeyboardButton[] {"Speed 1"},
                                    new KeyboardButton[] {"Speed 2"},
                                    new KeyboardButton[] {"Speed 3"}, 
                                    new KeyboardButton[] {"Cancel"}, 
                                };

                                await bot.SendTextMessage(update.Message.Chat.Id, "Motor 1 :",false,false,0,replyKeyboardMarkup);
                                wizardStep.Add(chatId,11);
                            }
                            else if (msg == "/motor2")
                            {
                                ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup();
                                replyKeyboardMarkup.OneTimeKeyboard = true;
                                replyKeyboardMarkup.Keyboard = new KeyboardButton[][]
                                {
                                    new KeyboardButton[] {"Off"},
                                    new KeyboardButton[] {"Speed 1"},
                                    new KeyboardButton[] {"Speed 2"},
                                    new KeyboardButton[] {"Speed 3"},
                                    new KeyboardButton[] {"Cancel"},
                                };

                                await bot.SendTextMessage(update.Message.Chat.Id, "Motor 2 :", false, false, 0, replyKeyboardMarkup);
                                wizardStep.Add(chatId, 21);
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
                        await Task.Delay(1000);
                    }
                }
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
                            int chatId = update.Message.From.Id;
                            User currentUser = GetUserInMonitoring(chatId);

                            string log = "";
                            if (currentUser != null)
                            {
                                log = string.Format("{0} {1} : {2}", currentUser.FirstName, currentUser.LastName, msg);
                            }
                            else
                            {
                                log = string.Format("Anonymous : {0}", msg);
                            }

                            Console.WriteLine(log);

                            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

                            

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
                            else if (msg == "/restart")
                            {
                                var service =
        entities.Services.FirstOrDefault(x => x.ServiceName == "RestartServerFromTelegram");

                                var users = entities.UsersServicesPermissions.Where(x => x.ServiceId == service.ServiceId);

                                List<int> chatIds = new List<int>();

                                foreach (UsersServicesPermission u in users)
                                {
                                    var userBot = entities.Bots.Where(x => x.UserId == u.UserId);

                                    foreach (Bot bt in userBot)
                                    {
                                        if (bt.ChatId != null) chatIds.Add(bt.ChatId.Value);
                                    }
                                }

                                if (chatIds.Contains(chatId))
                                {
                                    await bot.SendTextMessage(chatId, "Server is restarting ... ");

                                    offset = update.Id + 1;
                                    await bot.GetUpdates(offset);

                                    Process.Start("shutdown.exe", "-f -r -t 0");
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "You don't have access to run this command");
                                }
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

                                int i = 1;
                                int count = result2.Count;

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
                                        for (int j = 1; j <= priority; j++)
                                        {
                                            emojiRating += "\u2B50";
                                        }
                                    }

                                    string output = string.Format(@"{0} Alarm {0}
Number : {1}/{2}
Item Name : {3}
Item Id : {4}
Category : {5}
Description : {6}
Status : {7}
Priority : {8}
Date : {9}", emojiAlarm, i,count, notification.NotificationItem.Item.ItemName, notification.NotificationItem.ItemId, category
                        , notification.NotificationItem.NotificationMsg, emojiStatus, emojiRating, notification.Time);

                                    await bot.SendTextMessage(update.Message.Chat.Id, output);

                                    i++;
                                    
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
                            else if (msg.StartsWith("/on "))
                            {
                                var service =
        entities.Services.FirstOrDefault(x => x.ServiceName == "TurnOnOffItems");

                                var users = entities.UsersServicesPermissions.Where(x => x.ServiceId == service.ServiceId);

                                List<int> chatIds = new List<int>();

                                foreach (UsersServicesPermission u in users)
                                {
                                    var userBot = entities.Bots.Where(x => x.UserId == u.UserId);

                                    foreach (Bot bt in userBot)
                                    {
                                        if (bt.ChatId != null) chatIds.Add(bt.ChatId.Value);
                                    }
                                }

                                if (chatIds.Contains(chatId))
                                {
                                    var parts = msg.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                    if (parts.Length == 2)
                                    {
                                        string idStr = parts[1];
                                        int id;

                                        if (int.TryParse(idStr, out id))
                                        {
                                            var item = entities.Items.FirstOrDefault(x => x.ItemId == id);

                                            if (item.InOut == (int)InOut.Output)
                                            {
                                                ProcessDataService processDataService = new ProcessDataService();
                                                var resultOfRun = processDataService.On(item.Location);

                                                if (resultOfRun)
                                                {
                                                    await bot.SendTextMessage(update.Message.Chat.Id, "Successfull");
                                                }
                                                else
                                                {
                                                    await bot.SendTextMessage(update.Message.Chat.Id, "Failed");
                                                }
                                            }
                                            else
                                            {
                                                await bot.SendTextMessage(update.Message.Chat.Id, "This type of item can not be turned on");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "You don't have access to run this command");
                                }
                                
                            }
                            else if (msg.StartsWith("/off "))
                            {
                                var service =
        entities.Services.FirstOrDefault(x => x.ServiceName == "TurnOnOffItems");

                                var users = entities.UsersServicesPermissions.Where(x => x.ServiceId == service.ServiceId);

                                List<int> chatIds = new List<int>();

                                foreach (UsersServicesPermission u in users)
                                {
                                    var userBot = entities.Bots.Where(x => x.UserId == u.UserId);

                                    foreach (Bot bt in userBot)
                                    {
                                        if (bt.ChatId != null) chatIds.Add(bt.ChatId.Value);
                                    }
                                }

                                if (chatIds.Contains(chatId))
                                {
                                    var parts = msg.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                    if (parts.Length == 2)
                                    {
                                        string idStr = parts[1];
                                        int id;

                                        if (int.TryParse(idStr, out id))
                                        {
                                            var item = entities.Items.FirstOrDefault(x => x.ItemId == id);

                                            if (item.InOut == (int)InOut.Output)
                                            {
                                                ProcessDataService processDataService = new ProcessDataService();
                                                var resultOfRun = processDataService.Off(item.Location);

                                                if (resultOfRun)
                                                {
                                                    await bot.SendTextMessage(update.Message.Chat.Id, "Successfull");
                                                }
                                                else
                                                {
                                                    await bot.SendTextMessage(update.Message.Chat.Id, "Failed");
                                                }
                                            }
                                            else
                                            {
                                                await bot.SendTextMessage(update.Message.Chat.Id, "This type of item can not be turned off");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await bot.SendTextMessage(chatId, "You don't have access to run this command");
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

        public static User GetUserInTechnicalFanCoil(int chatId)
        {
            try
            {
                var entities = new IndustrialMonitoringEntities();

                var fanCoilBotRow = entities.FanCoilBots.FirstOrDefault(x => x.ChatId == chatId);

                User result = fanCoilBotRow.User;

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogTelegramBot(ex);
            }

            return null;
        }

        public static User GetUserInMonitoring(int chatId)
        {
            try
            {
                var entities = new IndustrialMonitoringEntities();

                var botRow = entities.Bots.FirstOrDefault(x => x.ChatId == chatId);

                User result = botRow.User;

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogTelegramBot(ex);
            }

            return null;
        }

        public static List<int> RecieveReportForTechnicalFanCoilChatIds()
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogTelegramBot(ex);
            }
            

            return new List<int>();
        }

        public static void StartFanCoilMobileServer()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, FanCoilPort);

            socket.Bind(endPoint);
            socket.Listen(10);

            while (true)
            {
                try
                {
                    Console.WriteLine("waiting for new connection...");

                    Socket newSocket = socket.Accept();
                    ThreadPool.QueueUserWorkItem((state => OnNewSocketAccept(newSocket)));
                }
                catch (Exception ex)
                {
                    
                    Logger.LogTelegramBot(ex);
                }

            }
        }

        public static void OnNewSocketAccept(Socket newSocket)
        {
            try
            {
                Console.WriteLine("new connection...");

                // first get length of data
                byte[] lengthB = new byte[4];
                newSocket.Receive(lengthB);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lengthB);
                }

                // length of data
                int length = BitConverter.ToInt32(lengthB, 0);

                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];

                int readBytes = newSocket.Receive(buffer);
                MemoryStream memoryStream = new MemoryStream();

                while (length > memoryStream.Length)
                {
                    if (readBytes > 0)
                    {
                        memoryStream.Write(buffer, 0, readBytes);
                    }

                    int available = newSocket.Available;

                    if (available > 0)
                    {
                        readBytes = newSocket.Receive(buffer);
                    }
                    else
                    {
                        readBytes = 0;
                    }
                }

                Console.WriteLine("data received...");

                BinaryFormatter formatter = new BinaryFormatter();
                memoryStream.Position = 0;
                Request request = (Request)formatter.Deserialize(memoryStream);
                memoryStream.Close();

                RemoteMethod methodNumber = request.MethodNumber;
                Response response = new Response();

                if (methodNumber == RemoteMethod.Echo)
                {
                    response.Result = request.Parameter;
                }
                else if (methodNumber == RemoteMethod.GetStatus2)
                {
                    TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();

                    // Dictionary<int,int>
                    var status = technicalFanCoil.GetStatus2();
                    response.Result = status;
                }
                else if (methodNumber == RemoteMethod.SetStatus)
                {
                    TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();

                    SetStatusWrapper value = (SetStatusWrapper) request.Parameter;
                    bool result = technicalFanCoil.SetStatus(value);

                    if (result)
                    {
                        ThreadPool.QueueUserWorkItem(obj =>
                        {
                            var bot = new Telegram.Bot.Api("208761880:AAHxUZCc5z0g2dJDrgbbMEWk7r1t_IoKiAw");
                            UserService userService=new UserService();
                            var currentUser = userService.GetUserFromSession(value.SessionKey);

                            var chatIds = RecieveReportForTechnicalFanCoilChatIds();
                            string report = string.Format("Status is changed by {0} {1}", currentUser.FirstName, currentUser.LastName);

                            foreach (var id in chatIds)
                            {
                                 bot.SendTextMessage(id, report);
                                 bot.SendTextMessage(id, technicalFanCoil.GetStatus());
                            }
                        });
                    }

                    // bool
                    response.Result = result;
                }
                else if (methodNumber == RemoteMethod.Authorize)
                {
                    UserService userService = new UserService();
                    AuthorizeWrapper wrapper = (AuthorizeWrapper)request.Parameter;
                    string userName = wrapper.UserName;
                    string password = wrapper.Password;

                    bool result = userService.Authorize(userName, password);

                    // bool
                    response.Result = result;
                }
                else if (methodNumber == RemoteMethod.AuthorizeAndGetSession)
                {
                    UserService userService=new UserService();
                    AuthorizeWrapper wrapper = (AuthorizeWrapper)request.Parameter;
                    string userName = wrapper.UserName;
                    string password = wrapper.Password;

                    string result = userService.AuthorizeAndGetSession(userName, password);

                    response.Result = result;
                }

                formatter = new BinaryFormatter();
                memoryStream = new MemoryStream();

                if (response.Result != null)
                {
                    formatter.Serialize(memoryStream, response);

                    byte[] dataBytes = memoryStream.ToArray();

                    int dataLength = dataBytes.Length;
                    // length of data in bytes
                    byte[] dataLengthB = BitConverter.GetBytes(dataLength);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(dataLengthB);
                    }

                    // first send length
                    newSocket.Send(dataLengthB);

                    // send data
                    newSocket.Send(dataBytes);

                    memoryStream.Close();
                    newSocket.Close();

                    Console.WriteLine("data sent...");
                }
                else
                {
                    memoryStream.Close();
                    newSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogMobileServer(ex);
                Console.WriteLine(ex.Message);
            }

        }
    }
}
