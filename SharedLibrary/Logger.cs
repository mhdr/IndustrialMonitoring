using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public static class Logger
    {
        public static void LogTelegramBot(Exception ex)
        {
            string path = @"C:\Logs\TelegramBot.log";
            Log(path, ex);
        }

        public static void LogMonitoringServiceLibrary(Exception ex)
        {
            string path = @"C:\Logs\MonitoringServiceLibrary.log";
            Log(path,ex);
        }

        public static void LogIndustrialMonitoring(Exception ex)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = @"C:\Logs\IndustrialMonitoring.log";
            string path = Path.Combine(folder, fileName);
            
            Log(path, ex);
        }

        public static void LogBacnetTest(Exception ex)
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

        private static void Log(string filePath, Exception ex)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(fileStream);
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

        private static void Log(string filePath, string txt)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(fileStream);
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
}
