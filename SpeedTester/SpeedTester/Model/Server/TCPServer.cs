using SpeedTester.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SpeedTester
{
    public delegate void StatsUpdateDelegate(ServerStats serverStats);
    class TCPServer : ServerBase
    {
        protected Socket serverSocket;
        protected Socket clientSocket;
        public TCPServer(IPAddress ipAddress, int port) : base(ipAddress, port) { }
        public Socket InitSocket()
        {
            Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            newSocket.Bind(localEndPoint);
            return newSocket;
        }
        public override void Run()
        {
            isRunning = true;
            try
            {
                serverSocket = InitSocket();
                serverSocket.Listen(10);
                while (isRunning)
                {
                    clientSocket = serverSocket.Accept();
                    try
                    {
                        byte[] fromClient = new byte[clientSocket.ReceiveBufferSize];
                        int length = clientSocket.Receive(fromClient);
                        int dataSize = Int32.Parse(Encoding.UTF8.GetString(fromClient).Substring(5, length));
                        clientSocket.ReceiveBufferSize = dataSize;
                        serverStats = new ServerStats();
                        serverStats.DataSize = dataSize;
                        ClientHandling(dataSize);
                    }
                    catch { }
                    clientSocket.Close();
                }
            }
            catch
            {
                isRunning = false;
            }
        }

        public override void RequestStop()
        {
            isRunning = false;
            if (serverSocket != null)
            {
                serverSocket.Close();
            }
        }

        protected void ClientHandling (int dataSize)
        {
            byte[] fromClient = new byte[dataSize];
            serverStats.DataSize = dataSize;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                while (!(clientSocket.Poll(1, SelectMode.SelectRead) && clientSocket.Available == 0))
                {
                    serverStats.TotalSize = serverStats.TotalSize + dataSize;
                    clientSocket.Receive(fromClient);
                    serverStats.TransmissionTime = (int)watch.ElapsedMilliseconds;
                    OnStatsUpdate(serverStats.ShallowCopy());
                }
            }
            catch(SocketException) { }
        }
    }
}
