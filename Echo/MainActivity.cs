using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharedLibrarySocket;
using SharedLibrarySocket.Warpper;

namespace Echo
{
    [Activity(Label = "Echo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button buttonEcho;
        private TextView textViewResult;
        private Button buttonEchoExternal;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            buttonEcho = FindViewById<Button>(Resource.Id.buttonEcho);
            textViewResult = FindViewById<TextView>(Resource.Id.textViewResult);
            buttonEchoExternal = FindViewById<Button>(Resource.Id.buttonEchoExternal);

            buttonEcho.Click += Button_Click;
            buttonEchoExternal.Click += ButtonEchoExternal_Click;
        }

        private void ButtonEchoExternal_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Echo();

                if (result)
                {
                    textViewResult.Text = "Ok";
                    textViewResult.SetTextColor(Color.Green);
                }
                else
                {
                    textViewResult.Text = "Failed";
                    textViewResult.SetTextColor(Color.Red);
                }
            }
            catch (Exception)
            {
                textViewResult.Text = "Failed";
                textViewResult.SetTextColor(Color.Red);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Echo2();

                if (result)
                {
                    textViewResult.Text = "Ok";
                    textViewResult.SetTextColor(Color.Green);
                }
                else
                {
                    textViewResult.Text = "Failed";
                    textViewResult.SetTextColor(Color.Red);
                }
            }
            catch (Exception)
            {
                textViewResult.Text = "Failed";
                textViewResult.SetTextColor(Color.Red);
            }
        }

        private bool Echo2()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 10000;
            socket.Connect("172.20.63.234", 14200);
            //socket.Connect("5.22.198.62", 4200);

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            Request request = new Request();
            request.MethodNumber = RemoteMethod.Echo;
            request.Parameter = "Ok";

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

            if (result == "Ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Echo()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 10000;
            //socket.Connect("172.20.63.234", 14001);
            socket.Connect("5.22.198.62", 14200);

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            Request request = new Request();
            request.MethodNumber = RemoteMethod.Echo;
            request.Parameter = "Ok";

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

            if (result == "Ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
}

