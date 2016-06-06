using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public static class Logger
    {
        public static void LogTelegramBot(Exception ex, [CallerMemberName] string callingMethod = "",
        [CallerFilePath] string callingFilePath = "",
        [CallerLineNumber] int callingFileLineNumber = 0)
        {
            string path = @"C:\Logs\TelegramBot.log";
            Log(path, ex);
        }

        public static void LogMonitoringServiceLibrary(Exception ex, [CallerMemberName] string callingMethod = "",
        [CallerFilePath] string callingFilePath = "",
        [CallerLineNumber] int callingFileLineNumber = 0)
        {
            string path = @"C:\Logs\MonitoringServiceLibrary.log";
            Log(path, ex);
        }

        public static void LogMobileServer(Exception ex, [CallerMemberName] string callingMethod = "",
[CallerFilePath] string callingFilePath = "",
[CallerLineNumber] int callingFileLineNumber = 0)
        {
            string path = @"C:\Logs\MobileServer.log";
            Log(path, ex);
        }

        public static void LogMonitoringServiceLibrary2(Exception ex, [CallerMemberName] string callingMethod = "",
[CallerFilePath] string callingFilePath = "",
[CallerLineNumber] int callingFileLineNumber = 0)
        {
            string path = @"C:\Logs\MonitoringServiceLibrary2.log";
            Log(path, ex);
        }

        public static void LogIndustrialMonitoring(Exception ex, [CallerMemberName] string callingMethod = "",
        [CallerFilePath] string callingFilePath = "",
        [CallerLineNumber] int callingFileLineNumber = 0)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = @"C:\Logs\IndustrialMonitoring.log";
            string path = Path.Combine(folder, fileName);

            Log(path, ex);
        }

        public static void LogBacnetTest(Exception ex, [CallerMemberName] string callingMethod = "",
        [CallerFilePath] string callingFilePath = "",
        [CallerLineNumber] int callingFileLineNumber = 0)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = @"C:\Logs\BacnetTest.log";
            string path = Path.Combine(folder, fileName);

            Log(path, ex);
        }

        public static void LogNetworkInterfaces(string txt)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = @"C:\Logs\NetworkInterfaces.log";
            string path = Path.Combine(folder, fileName);

            Log(path, txt);
        }

        private static void Log(string filePath, Exception ex, string callingMethod = "",
        string callingFilePath = "",
        int callingFileLineNumber = 0)
        {
            try
            {
                var padlock = new object();

                lock (padlock)
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.Append);
                    StreamWriter writer = new StreamWriter(fileStream);

                    string lineMsg = string.Format("File : {0} , Method : {1} , Line : {2}", callingFilePath, callingMethod,
                        callingFileLineNumber);
                    writer.WriteLine(lineMsg);
                    writer.WriteLine("***");

                    writer.WriteLine(ex.ToString());
                    writer.WriteLine();
                    writer.WriteLine("Date : {0}", DateTime.Now);
                    writer.WriteLine();
                    writer.WriteLine("-------------------------------------");
                    writer.WriteLine();
                    writer.Flush();
                    writer.Close();
                    fileStream.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }

        private static void Log(string filePath, string txt, string callingMethod = "",
        string callingFilePath = "",
        int callingFileLineNumber = 0)
        {
            try
            {
                var padlock = new object();

                lock (padlock)
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.Append);
                    StreamWriter writer = new StreamWriter(fileStream);

                    string lineMsg = string.Format("File : {0} , Method : {1} , Line : {2}", callingFilePath, callingMethod,
            callingFileLineNumber);
                    writer.WriteLine(lineMsg);
                    writer.WriteLine("***");

                    writer.WriteLine(txt);
                    writer.WriteLine();
                    writer.WriteLine("Date : {0}", DateTime.Now);
                    writer.WriteLine();
                    writer.WriteLine("-------------------------------------");
                    writer.WriteLine();
                    writer.Flush();
                    writer.Close();
                    fileStream.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
