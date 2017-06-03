using SpeedTester.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SpeedTester
{
    public delegate void StatsUpdateDelegate(ServerStats serverStats);
    class TCPServer
    {
        public StatsUpdateDelegate OnStatsUpdate;
        bool isRunning = false;
        IPAddress ipAddress;
        int port;
        Socket serverSocket;
        Socket clientSocket;
        ServerStats tcpStats;
        public TCPServer(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public Socket InitServer()
        {
            Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            newSocket.Bind(localEndPoint);
            return newSocket;
        }

        public void Run()
        {
            isRunning = true;
            try
            {
                serverSocket = InitServer();
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
                        Console.WriteLine("Message received from client: " + dataSize);
                        tcpStats = new ServerStats();
                        tcpStats.TCPDataSize = dataSize.ToString();
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

        public void RequestStop()
        {
            isRunning = false;
            serverSocket.Close();
        }

        private void ClientHandling (int dataSize)
        {
            byte[] fromClient = new byte[dataSize];
            tcpStats.TCPDataSize = dataSize.ToString();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                while (!(clientSocket.Poll(1, SelectMode.SelectRead) && clientSocket.Available == 0))
                {
                    tcpStats.TCPTotalSize = (Int32.Parse(tcpStats.TCPTotalSize) + dataSize).ToString();
                    clientSocket.Receive(fromClient);
                    tcpStats.TCPTransmissionTime = watch.ElapsedMilliseconds.ToString();
                    OnStatsUpdate(tcpStats);
                }
            }
            catch(SocketException) { }
        }
    }
}
