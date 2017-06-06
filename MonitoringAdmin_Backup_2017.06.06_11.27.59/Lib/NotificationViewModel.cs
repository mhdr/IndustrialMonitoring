using SharedLibrary;

namespace MonitoringAdmin.Lib
{
    public class NotificationViewModel
    {
        public NotificationType Type { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public string Message { get; set; }
        public int Priority { get; set; }
    }
}
