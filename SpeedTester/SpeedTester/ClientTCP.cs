using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester
{
    class ClientTCP
    {
        bool isRunning = false;
        IPAddress ipAddress;
        int port;

        public ClientTCP(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Run()
        {
            Console.WriteLine("Failed to connect to a server");
            Socket s = null;
            try
            {
                s = InitConnectionSocket();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect to a server");
            }
            if (s != null)
            {
                WorkWithServer(s);
                s.Close();
            }
        }

        public Socket InitConnectionSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ipAddress, port);
            Console.WriteLine("Connection succesful");
            return s;
        }

        public static void WorkWithServer(Socket s)
        {
            bool condition = false;
            String ts = "Enter message to server: ";
            do
            {
                byte[] toServer = Encoding.UTF8.GetBytes(ts);
                s.Send(toServer);
                Console.WriteLine("Message sent to server: " + ts);
                byte[] fromServer = new byte[s.ReceiveBufferSize];
                int length = s.Receive(fromServer);
                String fs = Encoding.UTF8.GetString(fromServer);
                if (!String.IsNullOrEmpty(fs))
                {
                    condition = !condition;
                    Console.WriteLine("Message received from server: " + fs.Substring(0, length));
                }
            } while (!condition);
        }
    }
}
