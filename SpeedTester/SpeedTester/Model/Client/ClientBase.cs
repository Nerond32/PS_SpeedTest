using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester.Model.Client
{
    public delegate void OnBrokenConnection();
    abstract class ClientBase
    {
        public OnBrokenConnection OnBrokenConnection;
        protected bool isRunning = false;
        protected IPAddress ipAddress;
        protected int port;
        protected Socket clientSocket;
        protected int bufferSize;
        public ClientBase(IPAddress ipAddress, int port, int bufferSize, bool nagleAlgorithmEnabled)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.bufferSize = bufferSize;
            clientSocket = InitConnectionSocket(nagleAlgorithmEnabled);
        }
        public abstract void Run();
        public abstract void RequestStop();
        protected abstract Socket InitConnectionSocket(bool nagleAlgorithmEnabled);
    }
}
