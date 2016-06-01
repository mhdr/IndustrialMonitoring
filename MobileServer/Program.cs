using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary;
using SharedLibrarySocket;

namespace MobileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 14001);

            socket.Bind(endPoint);
            socket.Listen(10);

            while (true)
            {
                Console.WriteLine("waiting for new connection...");

                Socket newSocket = socket.Accept();
                OnNewSocketAccept(newSocket);
            }
        }

        public static async void OnNewSocketAccept(Socket newSocket)
        {
            Console.WriteLine("new connection...");

            byte[] buffer = new byte[8*1024];

            int readBytes = newSocket.Receive(buffer);
            MemoryStream memoryStream = new MemoryStream();

            while (readBytes > 0)
            {
                memoryStream.Write(buffer, 0, readBytes);

                if (newSocket.Available > 0)
                {
                    readBytes = newSocket.Receive(buffer);
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("data received...");

            BinaryFormatter formatter = new BinaryFormatter();
            memoryStream.Position = 0;
            Request request = (Request) formatter.Deserialize(memoryStream);
            memoryStream.Close();

            int methodNumber = request.MethodNumber;
            Response response=new Response();

            if (methodNumber == 1)
            {
                TechnicalFanCoil technicalFanCoil=new TechnicalFanCoil();

                // Dictionary<int,int>
                var status = technicalFanCoil.GetStatus2();
                response.Result = status;
            }

            formatter = new BinaryFormatter();
            memoryStream = new MemoryStream();

            if (response.Result != null)
            {
                formatter.Serialize(memoryStream, response);

                newSocket.Send(memoryStream.ToArray());

                memoryStream.Close();
                newSocket.Close();

                Console.WriteLine("data sent...");
            }
        }
    }
}
