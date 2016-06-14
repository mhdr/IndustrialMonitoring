using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using SharedLibrarySocket;
using SharedLibrarySocket.Interfaces;
using SharedLibrarySocket.Warpper;
using TechnicalFanCoilAndroid.Lib;

namespace TechnicalFanCoilAndroid.RPC
{
    class UserService : IUserService
    {
        public bool Authorize(string userName, string password)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 10000;
            socket.Connect(Statics.IPAddress, Statics.Port);

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            AuthorizeWrapper authorizeWrapper=new AuthorizeWrapper();
            authorizeWrapper.UserName = userName;
            authorizeWrapper.Password = password;

            Request request = new Request();
            request.MethodNumber = RemoteMethod.Authorize;
            request.Parameter = authorizeWrapper;

            formatter.Serialize(memoryStream, request);

            byte[] dataBytes = memoryStream.ToArray();

            int dataLength = dataBytes.Length;
            // length of data in bytes
            byte[] dataLengthB = BitConverter.GetBytes(dataLength);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataLengthB);
            }

            // first send length
            socket.Send(dataLengthB);

            // send data
            int successfullSent = socket.Send(dataBytes);

            memoryStream = new MemoryStream();

            // first get length of data
            byte[] lengthB = new byte[4];
            socket.Receive(lengthB);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthB);
            }

            // length of data
            int length = BitConverter.ToInt32(lengthB, 0);

            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int readBytes = socket.Receive(buffer);

            while (length > memoryStream.Length)
            {
                if (readBytes > 0)
                {
                    memoryStream.Write(buffer, 0, readBytes);
                }

                int available = socket.Available;

                if (available > 0)
                {
                    readBytes = socket.Receive(buffer);
                }
                else
                {
                    readBytes = 0;
                }
            }

            formatter = new BinaryFormatter();

            // set position to 0 or create a new stream
            memoryStream.Position = 0;

            Response response = (Response)formatter.Deserialize(memoryStream);

            bool result = (bool)response.Result;

            memoryStream.Close();
            socket.Close();

            return result;
        }

        public string AuthorizeAndGetSession(string username, string password)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 10000;
            //socket.Connect("172.20.63.234", 14001);
            socket.Connect(Statics.IPAddress, Statics.Port);

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            AuthorizeWrapper authorizeWrapper = new AuthorizeWrapper();
            authorizeWrapper.UserName = username;
            authorizeWrapper.Password = password;

            Request request = new Request();
            request.MethodNumber = RemoteMethod.AuthorizeAndGetSession;
            request.Parameter = authorizeWrapper;

            formatter.Serialize(memoryStream, request);

            byte[] dataBytes = memoryStream.ToArray();

            int dataLength = dataBytes.Length;
            // length of data in bytes
            byte[] dataLengthB = BitConverter.GetBytes(dataLength);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataLengthB);
            }

            // first send length
            socket.Send(dataLengthB);

            // send data
            int successfullSent = socket.Send(dataBytes);

            memoryStream = new MemoryStream();

            // first get length of data
            byte[] lengthB = new byte[4];
            socket.Receive(lengthB);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthB);
            }

            // length of data
            int length = BitConverter.ToInt32(lengthB, 0);

            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int readBytes = socket.Receive(buffer);

            while (length > memoryStream.Length)
            {
                if (readBytes > 0)
                {
                    memoryStream.Write(buffer, 0, readBytes);
                }

                int available = socket.Available;

                if (available > 0)
                {
                    readBytes = socket.Receive(buffer);
                }
                else
                {
                    readBytes = 0;
                }
            }

            formatter = new BinaryFormatter();

            // set position to 0 or create a new stream
            memoryStream.Position = 0;

            Response response = (Response)formatter.Deserialize(memoryStream);

            string result = response.Result.ToString();

            memoryStream.Close();
            socket.Close();

            return result;
        }
    }
}