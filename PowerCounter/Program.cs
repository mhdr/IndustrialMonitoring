using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            GetData();
        }

        public static async void GetData()
        {
            HttpClient client = new HttpClient();

            string url = @"http://192.168.10.31/webMI/?read";

            Dictionary<string, string> values = new Dictionary<string, string>();

            values.Add("address", "I_PH1_255");

            var content = new FormUrlEncodedContent(values);


            while (true)
            {
                var response = await client.PostAsync(url, content);

                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseString);

                Thread.Sleep(1000);
            }
        }
    }
}
