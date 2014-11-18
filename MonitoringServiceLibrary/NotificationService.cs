using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary.ViewModels;

namespace MonitoringServiceLibrary
{
    public class NotificationService : INotificationService
    {
        public bool HasNotification(int itemId)
        {
            int count = 0;
            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

            List<NotificationItem> notificationItems = entities.NotificationItems.Where(x => x.ItemId == itemId).ToList();

            if (notificationItems == null || notificationItems.Count == 0)
            {
                return false;
            }

            foreach (var notificationItem in notificationItems)
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

        public List<Notification1> GetNotifications(int itemId)
        {
            List<Notification1> resultList=new List<Notification1>();

            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

            List<NotificationItem> notificationItems = entities.NotificationItems.Where(x => x.ItemId == itemId).ToList();

            if (notificationItems == null || notificationItems.Count == 0)
            {
                return null;
            }

            foreach (var notificationItem in notificationItems)
            {
                NotificationItemsLogLatest notificationItemsLogLatest =
                    entities.NotificationItemsLogLatests.FirstOrDefault(
                        x => x.NotificationId == notificationItem.NotificationId);

                if (notificationItemsLogLatest.Value)
                {
                    Notification1 notification1=new Notification1(notificationItem);
                    resultList.Add(notification1);
                }
            }

            return resultList;
        }

        public bool SystemHasNotification(int userId)
        {
            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();

            var notifications = entities.NotificationItemsLogLatests.ToList();

            if (notifications == null || notifications.Count == 0)
            {
                return false;
            }

            List<int> ItemIds=new List<int>();

            foreach (NotificationItemsLogLatest notification in notifications)
            {
                ItemIds.Add(notification.NotificationItem.ItemId);
            }

            List<int> ItemIdsForCurrentUser=new List<int>();

            foreach (int itemId in ItemIds)
            {
                if (entities.UsersItemsPermissions.Any(x => x.UserId == userId && x.ItemId == itemId))
                {
                    ItemIdsForCurrentUser.Add(itemId);
                }
            }

            if (ItemIdsForCurrentUser.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
