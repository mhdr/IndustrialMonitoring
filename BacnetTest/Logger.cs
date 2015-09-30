using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BACnetTest
{
    public static class Logger
    {
        public static void LogBacnetTest(Exception ex)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = "BacnetTest.log";
            string path = Path.Combine(folder, fileName);

            Log(path, ex);
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
    }
}
