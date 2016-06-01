using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using SharedLibrarySocket;

namespace TechnicalFanCoilAndroid
{
    public interface ITechnicalFanCoil
    {
        Dictionary<int, int> GetStatus2();
    }

    class TechnicalFanCoil : ITechnicalFanCoil
    {
        public Dictionary<int, int> GetStatus2()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect("172.20.63.234", 14001);

                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();

                Request request = new Request();
                request.MethodNumber = 1;

                formatter.Serialize(memoryStream, request);

                byte[] dataBytes = memoryStream.ToArray();
                socket.Send(dataBytes);

                memoryStream = new MemoryStream();
                byte[] buffer = new byte[1024 * 8];
                int readBytes = socket.Receive(buffer);

                while (readBytes > 0)
                {
                    memoryStream.Write(buffer, 0, readBytes);

                    if (socket.Available > 0)
                    {
                        readBytes = socket.Receive(buffer);
                    }
                    else
                    {
                        break;
                    }
                }

                formatter = new BinaryFormatter();

                // set position to 0 or create a new stream
                memoryStream.Position = 0;

                Response response = (Response)formatter.Deserialize(memoryStream);

                Dictionary<int, int> result = (Dictionary<int, int>)response.Result;

                memoryStream.Close();
                socket.Close();

                return result;
            }
            catch (Exception)
            {
                
            }

            return null;
        }
    }
}