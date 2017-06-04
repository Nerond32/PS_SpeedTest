using SpeedTester.Model.Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SpeedTester
{
    class TCPClient : ClientBase
    {
        public TCPClient(IPAddress ipAddress, int port, int bufferSize, bool nagleAlgorithmEnabled) : base(ipAddress, port, bufferSize, nagleAlgorithmEnabled) { }
        public override void RequestStop()
        {
            isRunning = false;
        }

        public override void Run()
        {
            try
            {
                WorkWithServer(bufferSize);
            }
            catch (SocketException e)
            {
                OnBrokenConnection();
            }
            clientSocket.Close();
        }

        protected override Socket InitConnectionSocket(bool nagleAlgorithmEnabled)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.NoDelay = !nagleAlgorithmEnabled;
            s.Connect(ipAddress, port);
            return s;
        }

        private void WorkWithServer(int bufferSize)
        {
            isRunning = true;
            String ts = "SIZE:"+bufferSize;
            byte[] toServer = Encoding.UTF8.GetBytes(ts);
            clientSocket.Send(toServer);
            byte[] bufferContent = Encoding.UTF8.GetBytes(GenerateContent(bufferSize));
            do
            {
                clientSocket.Send(bufferContent);
            } while (isRunning);
        }

        private static string GenerateContent(int bufferSize)
        {
            string content="";
            for(int i = 0; i < bufferSize; i++)
            {
                content += "X";
            }
            return content;
        }
    }
}
