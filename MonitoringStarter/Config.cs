using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonitoringStarter
{
    public class Config
    {
        public string ConsoleHostPath;
        public string TelegramBotPath;
        public string SystemManagerPath;

        public static Config LoadConfig()
        {
            try
            {
                string path = "Config.json";
                StreamReader streamReader = new StreamReader(path);
                string jsonStr = streamReader.ReadToEnd();
                Config config = JsonConvert.DeserializeObject<Config>(jsonStr);
                streamReader.Close();
                return config;
            }
            catch (Exception)
            {
                // TODO behtare ke to log ham zakhire beshe
                Config config = new Config();
                config.Initialize();
                return config;
            }
        }

        public void Initialize()
        {
            this.ConsoleHostPath = "";
            this.TelegramBotPath = "";
            this.SystemManagerPath = "";
        }
    }
}
