using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester.Model
{
    class UDPClient
    {
        bool isRunning = false;
        IPAddress ipAddress;
        int port;
        Socket clientSocket;
        int bufferSize;

        public UDPClient(IPAddress ipAddress, int port, int bufferSize)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.bufferSize = bufferSize;
            clientSocket = InitConnectionSocket();
        }

        public Socket InitConnectionSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            return s;
        }

        public void Run()
        {
            WorkWithServer(bufferSize);
            clientSocket.Close();
        }

        public void WorkWithServer(int bufferSize)
        {
            isRunning = true;
            IPEndPoint sending_end_points = new IPEndPoint(ipAddress, port);
            String ts = "SIZE:" + bufferSize;
            byte[] toServer = Encoding.UTF8.GetBytes(ts);
            clientSocket.SendTo(toServer, sending_end_points);
            byte[] bufferContent = Encoding.UTF8.GetBytes(GenerateContent(bufferSize));
            do
            {
                clientSocket.SendTo(bufferContent, bufferSize, SocketFlags.None, sending_end_points);
            } while (isRunning);
        }
        private static string GenerateContent(int bufferSize)
        {
            string content = "";
            for (int i = 0; i < bufferSize; i++)
            {
                content += "X";
            }
            return content;
        }

        public void RequestStop()
        {
            isRunning = false;
        }
    }
}
