﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary
{
    public class NotificationService : INotificationService
    {
        public bool HasNotification(int itemId)
        {
            int count = 0;
            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

            var itemIds = entities.NotificationItems.Where(x => x.ItemId == itemId);

            foreach (var notificationItem in itemIds)
            {
                NotificationItemsLogLatest notificationItemsLogLatest =
                    entities.NotificationItemsLogLatests.FirstOrDefault(
                        x => x.NotificationId == notificationItem.NotificationId);

                if (notificationItemsLogLatest.Value)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return true;
            }

            return false;
        }
    }
}