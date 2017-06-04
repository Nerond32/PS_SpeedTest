using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SpeedTester.Model
{
    public delegate void StatsUpdateDelegate(ServerStats serverStats);
    class UDPServer : ServerBase
    {
        protected UdpClient listener;
        public UDPServer(IPAddress ipAddress, int port) :base(ipAddress, port) { }
        public override void Run()
        {
            listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(ipAddress, port);
            isRunning = true;
            serverStats = new ServerStats();
            try
            {
                Stopwatch watch = new Stopwatch();
                while (isRunning)
                {
                    byte[] receive_byte_array = listener.Receive(ref groupEP);
                    string received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
                    try
                    {
                        if (received_data.Substring(0, 5) == "SIZE:")
                        {
                            serverStats = new ServerStats();
                            watch.Restart();
                            listener.Client.ReceiveBufferSize = Int32.Parse(received_data.Substring(5, received_data.Length - 5));
                            serverStats.DataSize = listener.Client.ReceiveBufferSize;
                            if (listener.Client.ReceiveBufferSize < 10) listener.Client.ReceiveBufferSize = 10;
                            if (listener.Client.ReceiveBufferSize > 65536) listener.Client.ReceiveBufferSize = 65536;
                        }
                    }
                    catch (ArgumentOutOfRangeException) { }
                    serverStats.TransmissionTime = (int)watch.ElapsedMilliseconds;
                    serverStats.TotalSize += listener.Client.ReceiveBufferSize;
                    OnStatsUpdate(serverStats.ShallowCopy());
                }
            }
            catch
            {
                isRunning = false;
            }
            listener.Close();
        }

        public override void RequestStop()
        {
            listener.Close();
            isRunning = false;
        }
    }
}
