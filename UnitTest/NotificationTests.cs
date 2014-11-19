using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.NotificationServiceReference;

namespace UnitTest
{
    [TestClass]
    public class NotificationTests
    {
        [TestMethod]
        public void GetNotificationLogs01()
        {
            NotificationServiceReference.NotificationServiceClient notificationServiceClient=new NotificationServiceClient();
            DateTime startTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);
            DateTime endTime = DateTime.Now;
            var result= notificationServiceClient.GetNotificationLogs(1, startTime, endTime);

            Console.Write("");
        }
    }
}
