namespace TechnicalFanCoilAndroid.Lib
{
    public static class Statics
    {
        public static string DatabaseFilePath;
        public static int Port = 4200;
        public static string IPAddress = "5.22.198.62";

        //public static int Port = 14001;
        //public static string IPAddress = "172.20.63.234";

        public static string GetConnectionString()
        {
            return string.Format("Data Source={0};Version=3;", DatabaseFilePath);
        }
    }
}