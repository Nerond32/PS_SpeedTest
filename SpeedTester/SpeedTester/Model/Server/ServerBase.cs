using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester.Model
{
    abstract class ServerBase
    {
        public StatsUpdateDelegate OnStatsUpdate;
        protected ServerStats serverStats;
        protected bool isRunning = false;
        protected IPAddress ipAddress;
        protected int port;
        public ServerBase(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }
        public abstract void Run();
        public abstract void RequestStop();
    }
}
