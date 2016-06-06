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
using SharedLibrary;
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
            try
            {
                Console.WriteLine("new connection...");

                // first get length of data
                byte[] lengthB = new byte[4];
                newSocket.Receive(lengthB);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lengthB);
                }

                // length of data
                int length = BitConverter.ToInt32(lengthB, 0);

                int bufferSize = 1024 * 8;
                byte[] buffer = new byte[bufferSize];

                int readBytes = newSocket.Receive(buffer);
                MemoryStream memoryStream = new MemoryStream();

                while (length > memoryStream.Length)
                {
                    if (readBytes > 0)
                    {
                        memoryStream.Write(buffer, 0, readBytes);
                    }

                    int available = newSocket.Available;

                    if (available > 0)
                    {
                        readBytes = newSocket.Receive(buffer);
                    }
                    else
                    {
                        readBytes = 0;
                    }
                }

                Console.WriteLine("data received...");

                BinaryFormatter formatter = new BinaryFormatter();
                memoryStream.Position = 0;
                Request request = (Request)formatter.Deserialize(memoryStream);
                memoryStream.Close();

                int methodNumber = request.MethodNumber;
                Response response = new Response();

                if (methodNumber == 1)
                {
                    TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();

                    // Dictionary<int,int>
                    var status = technicalFanCoil.GetStatus2();
                    response.Result = status;
                }
                else if (methodNumber == 2)
                {
                    TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();

                    Dictionary<int, int> dic = (Dictionary<int, int>)request.Parameter;
                    bool result = technicalFanCoil.SetStatus(dic);

                    // bool
                    response.Result = result;
                }

                formatter = new BinaryFormatter();
                memoryStream = new MemoryStream();

                if (response.Result != null)
                {
                    formatter.Serialize(memoryStream, response);

                    byte[] dataBytes = memoryStream.ToArray();

                    int dataLength = dataBytes.Length;
                    // length of data in bytes
                    byte[] dataLengthB = BitConverter.GetBytes(dataLength);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(dataLengthB);
                    }

                    // first send length
                    newSocket.Send(dataLengthB);

                    // send data
                    newSocket.Send(dataBytes);

                    memoryStream.Close();
                    newSocket.Close();

                    Console.WriteLine("data sent...");
                }
                else
                {
                    memoryStream.Close();
                    newSocket.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogMobileServer(ex);
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
