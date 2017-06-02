using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester
{
    class TCPServer
    {
        bool isRunning = false;
        IPAddress ipAddress;
        int port;
        Socket clientSocket;
        public TCPServer(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }
        public Socket InitServer()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            s.Bind(localEndPoint);
            return s;
        }
        public void Run()
        {
            isRunning = true;
            Socket serverSocket = InitServer();
            Console.WriteLine("Starting server at: " + serverSocket.LocalEndPoint);
            serverSocket.Listen(10);
            while (isRunning)
            {
                Console.WriteLine("Waiting for new connection.");
                clientSocket = serverSocket.Accept();
                Console.WriteLine("Connection accepted from " + clientSocket.RemoteEndPoint.ToString());
                byte[] fromServer = new byte[clientSocket.ReceiveBufferSize];
                int length = clientSocket.Receive(fromServer);
                String ts = Encoding.UTF8.GetString(fromServer).Substring(0, length);
                Console.WriteLine("Message received from client: " + ts);
                clientSocket.Send(Encoding.UTF8.GetBytes(ts));
                clientSocket.Close();
            }
            serverSocket.Close();
            Console.ReadLine();
        }
        public void RequestStop()
        {
            isRunning = false;
            //clientSocket.Shutdown(SocketShutdown.Both);
            //clientSocket.Close();
            //clientSocket.LingerState = new LingerOption(true, 0);
            //clientSocket.Close();
        }
    }
}
