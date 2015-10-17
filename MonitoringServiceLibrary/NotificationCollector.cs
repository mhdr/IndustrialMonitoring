using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;

namespace MonitoringServiceLibrary
{
    public class NotificationCollector
    {
        private Thread _runningThread;
        private bool _stopThread;
        private bool _isThreadRunning;

        public NotificationCollector()
        {
            RunningThread = new Thread(CheckNotifications);
        }

        public Thread RunningThread
        {
            get { return _runningThread; }
            set { _runningThread = value; }
        }

        private bool StopThread
        {
            get { return _stopThread; }
            set { _stopThread = value; }
        }

        public bool IsThreadRunning
        {
            get { return _isThreadRunning; }
            set { _isThreadRunning = value; }
        }

        public void Start()
        {
            if (IsThreadRunning == false)
            {
                this.StopThread = false;
                RunningThread.Start();                
            }
        }

        public void Stop()
        {
            this.StopThread = true;
        }

        public async void StartDelayedNotifications()
        {
            while (true)
            {
                

            }
        }

        private void CheckNotifications()
        {
            IsThreadRunning = true;

            while (!StopThread)
            {
                IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();
                List<NotificationItem> notificationItems = entities.NotificationItems.ToList();

                foreach (var notificationItem in notificationItems)
                {
                    try
                    {
                        entities = new IndustrialMonitoringEntities();
                        int notificationId = notificationItem.NotificationId;

                        NotificationItemsLogLatest notificationItemsLogLatest = entities.NotificationItemsLogLatests.FirstOrDefault(x => x.NotificationId == notificationId);

                        if (notificationItemsLogLatest == null)
                        {
                            notificationItemsLogLatest = new NotificationItemsLogLatest();
                            notificationItemsLogLatest.NotificationId = notificationId;
                            notificationItemsLogLatest.Value = false;
                            notificationItemsLogLatest.Time = DateTime.Now;

                            entities.NotificationItemsLogLatests.Add(notificationItemsLogLatest);
                            entities.SaveChanges();
                        }
                        else
                        {
                            ItemsLogLatest itemLogLatest =
            entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == notificationItem.ItemId);
                            
                            double currentValue = double.Parse(itemLogLatest.Value);

                            bool withoutNotification = false;

                            if (notificationItem.NotificationType == (int)NotificationType.Lower)
                            {
                                if (currentValue < notificationItem.High)
                                {
                                    withoutNotification = true;
                                }
                            }
                            else if (notificationItem.NotificationType == (int)NotificationType.Between)
                            {
                                if (currentValue > notificationItem.Low && currentValue < notificationItem.High)
                                {
                                    withoutNotification = true;
                                }
                            }
                            else if (notificationItem.NotificationType == (int)NotificationType.Higher)
                            {
                                if (currentValue > notificationItem.Low)
                                {
                                    withoutNotification = true;
                                }
                            }

                            if (withoutNotification)
                            {
                                if (notificationItemsLogLatest.Value == false)
                                {
                                    NotificationItemsLog notificationItemsLog = new NotificationItemsLog();
                                    notificationItemsLog.NotificationId = notificationId;
                                    notificationItemsLog.Value = true;
                                    notificationItemsLog.Time = DateTime.Now;

                                    entities.NotificationItemsLogs.Add(notificationItemsLog);
                                    entities.SaveChanges();

                                    var bot = NotificationsBot.Instance;
                                    bot.SendNotification(notificationItemsLog.NotificationLogId);
                                }
                            }
                            else
                            {
                                if (notificationItemsLogLatest.Value)
                                {
                                    if (notificationItem.IsDelayed != null && notificationItem.IsDelayed.Value)
                                    {
                                        int currentCount = 0;
                                        List<string> results=new List<string>();

                                        int numberOfDelay = 2;

                                        if (notificationItem.NumberOfDelayes != null)
                                        {
                                            numberOfDelay = notificationItem.NumberOfDelayes.Value;
                                        }

                                        while (currentCount<numberOfDelay)
                                        {
                                            ItemCollector itemCollector=new ItemCollector(notificationItem.Item);
                                            string currentResult =itemCollector.ReadValue(true);
                                            results.Add(currentResult);

                                            if (notificationItem.IntervalBetweenItems != null)
                                            {
                                                Thread.Sleep(notificationItem.IntervalBetweenItems.Value);
                                            }
                                            else
                                            {
                                                Thread.Sleep(1000);
                                            }

                                            currentCount++;
                                        }

                                        int numberOfWithoutNotifications = 0;

                                        foreach (string s in results)
                                        {
                                            currentValue = double.Parse(s);

                                            if (notificationItem.NotificationType == (int)NotificationType.Lower)
                                            {
                                                if (currentValue < notificationItem.High)
                                                {
                                                    numberOfWithoutNotifications++;
                                                }
                                            }
                                            else if (notificationItem.NotificationType == (int)NotificationType.Between)
                                            {
                                                if (currentValue > notificationItem.Low && currentValue < notificationItem.High)
                                                {
                                                    numberOfWithoutNotifications++;
                                                }
                                            }
                                            else if (notificationItem.NotificationType == (int)NotificationType.Higher)
                                            {
                                                if (currentValue > notificationItem.Low)
                                                {
                                                    numberOfWithoutNotifications++;
                                                }
                                            }
                                        }

                                        if (numberOfWithoutNotifications == notificationItem.NumberOfDelayes.Value)
                                        {
                                            withoutNotification = true;
                                        }
                                        else
                                        {
                                            withoutNotification = false;
                                        }
                                    }

                                    if (withoutNotification == false)
                                    {
                                        // we have a notification

                                        NotificationItemsLog notificationItemsLog = new NotificationItemsLog();
                                        notificationItemsLog.NotificationId = notificationId;
                                        notificationItemsLog.Value = false;
                                        notificationItemsLog.Time = DateTime.Now;

                                        entities.NotificationItemsLogs.Add(notificationItemsLog);
                                        entities.SaveChanges();

                                        var bot = NotificationsBot.Instance;
                                        bot.SendNotification(notificationItemsLog.NotificationLogId);
                                    }
                                }
                            }


                            notificationItemsLogLatest.Value = withoutNotification;
                            notificationItemsLogLatest.Time = DateTime.Now;
                            entities.SaveChanges();
                        }

                        Thread.Sleep(10);

                    }
                    catch (Exception ex)
                    {
                        Logger.LogMonitoringServiceLibrary(ex);
                    }
                }

                Thread.Sleep(1000);                
            }

            IsThreadRunning = false;
        }
    }
}
