using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester
{
    class TCPClient
    {
        bool isRunning = false;
        IPAddress ipAddress;
        int port;
        Socket clientSocket;
        int bufferSize;

        public TCPClient(IPAddress ipAddress, int port, int bufferSize)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.bufferSize = bufferSize;
        }

        public Socket InitConnectionSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ipAddress, port);
            Console.WriteLine("Connection succesful");
            return s;
        }

        public void Run()
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = InitConnectionSocket();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to a server");
            }
            if (clientSocket != null)
            {
                WorkWithServer(clientSocket, bufferSize);
                clientSocket.Close();
            }
        }

        

        public static void WorkWithServer(Socket s, int bufferSize)
        {
            bool isRunning = false;
            String ts = "SIZE:"+bufferSize;
            Console.WriteLine(ts);
            byte[] toServer = Encoding.UTF8.GetBytes(ts);
            s.Send(toServer);
            byte[] bufferContent = Encoding.UTF8.GetBytes(GenerateContent(bufferSize));
            do
            {
                s.Send(bufferContent);
            } while (!isRunning);
        }
        private static string GenerateContent(int bufferSize)
        {
            string content="";
            for(int i = 0; i < bufferSize; i++)
            {
                content += "X";
            }
            Console.WriteLine(content);
            return content;
        }
    }
}
