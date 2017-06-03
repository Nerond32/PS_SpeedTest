using SpeedTester.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpeedTester.ViewModel
{
    public delegate void IPPortDelegate(string ipAddress, int port);
    class ServerMenuViewModel : INotifyPropertyChanged
    {
        private static IPPortDelegate ipPortDelegate = setIpAndPort;
        //private static StatsUpdateDelegate statsDelegate = statsUpdate;
        private Thread tcpThread;
        private Thread udpThread;
        private TCPServer tcpServer;
        private bool running = false;
        private static IPAddress serverIPAddress;
        private static int serverPort;
        #region Binding Strings
        private string serverStatusButtonText = "Start server";
        private String tcpDataSize = "0";
        private String tcpTotalSize = "0";
        private String tcpTransmissionTime = "0";
        private String tcpTransmissionSpeed = "0";
        private String tcpLostData = "0";
        private String tcpTransmissionError = "0";
        private String udpDataSize = "0";
        private String udpTotalSize = "0";
        private String udpTransmissionTime = "0";
        private String udpTransmissionSpeed = "0";
        private String udpLostData = "0";
        private String udpTransmissionError = "0";
        #endregion
        #region Binding Properties
        public String ServerStatusButtonText
        {
            get
            {
                return serverStatusButtonText;
            }
            set
            {
                serverStatusButtonText = value;
                OnPropertyChanged("ServerStatusButtonText");
            }
        }
        public String TCPDataSize {
            get
            {
                return tcpDataSize;
            }
            set
            {
                tcpDataSize = value;
                OnPropertyChanged("TCPDataSize");
            }
        }
        public String TCPTotalSize
        {
            get
            {
                return tcpTotalSize;
            }
            set
            {
                tcpTotalSize = value;
                OnPropertyChanged("TCPTotalSize");
            }
        }
        public String TCPTransmissionTime
        {
            get
            {
                return tcpTransmissionTime;
            }
            set
            {
                tcpTransmissionTime = value;
                OnPropertyChanged("TCPTransmissionTime");
            }
        }
        public String TCPTransmissionSpeed
        {
            get
            {
                return tcpTransmissionSpeed;
            }
            set
            {
                tcpTransmissionSpeed = value;
                OnPropertyChanged("TCPTransmissionSpeed");
            }
        }
        public String TCPLostData
        {
            get
            {
                return tcpLostData;
            }
            set
            {
                tcpLostData = value;
                OnPropertyChanged("TCPLostData");
            }
        }
        public String TCPTransmissionError
        {
            get
            {
                return tcpTransmissionError;
            }
            set
            {
                tcpTransmissionError = value;
                OnPropertyChanged("TCPTransmissionError");
            }
        }
        public String UDPDataSize
        {
            get
            {
                return udpDataSize;
            }
            set
            {
                udpDataSize = value;
                OnPropertyChanged("UDPDataSize");
            }
        }
        public String UDPTotalSize
        {
            get
            {
                return udpTotalSize;
            }
            set
            {
                udpTotalSize = value;
                OnPropertyChanged("UDPTotalSize");
            }
        }
        public String UDPTransmissionTime
        {
            get
            {
                return udpTransmissionTime;
            }
            set
            {
                udpTransmissionTime = value;
                OnPropertyChanged("UDPTransmissionTime");
            }
        }
        public String UDPTransmissionSpeed
        {
            get
            {
                return udpTransmissionSpeed;
            }
            set
            {
                udpTransmissionSpeed = value;
                OnPropertyChanged("UDPTransmissionSpeed");
            }
        }
        public String UDPLostData
        {
            get
            {
                return udpLostData;
            }
            set
            {
                udpLostData = value;
                OnPropertyChanged("UDPLostData");
            }
        }
        public String UDPTransmissionError
        {
            get
            {
                return udpTransmissionError;
            }
            set
            {
                udpTransmissionError = value;
                OnPropertyChanged("UDPTransmissionError");
            }
        }
        #endregion 
        public event PropertyChangedEventHandler PropertyChanged;
        public static IPPortDelegate IpPortDelegate { get => ipPortDelegate; set => ipPortDelegate = value; }
        public ICommand StartStopServer { get { return new RelayCommand(Run); } }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Run()
        {
            try
            {
                if (running)
                {
                    running = false;
                    tcpServer.RequestStop();
                    ServerStatusButtonText = "Start Server";
                }
                else
                {
                    running = true;
                    tcpServer = new TCPServer(serverIPAddress, serverPort);
                    tcpServer.OnStatsUpdate += statsUpdate;
                    tcpThread = new Thread(tcpServer.Run);
                    tcpThread.Start();
                    ServerStatusButtonText = "Stop Server";
                }
            }
            catch(InvalidAsynchronousStateException e)
            {
                running = false;
                ServerStatusButtonText = "Start Server";
                MessageBox.Show("Invalid ip address or port entered!", "Input error");
            }
        }
        private static void setIpAndPort(string ipAddress, int port)
        {
            try
            {
                IPAddress tmpAddress = IPAddress.Parse(ipAddress);
                int tmpPort = port;
                serverIPAddress = tmpAddress;
                serverPort = tmpPort;
            }
            catch { }
        }
        private void statsUpdate(ServerStats serverStats)
        {
            TCPDataSize = serverStats.TCPDataSize;
            TCPTotalSize = serverStats.TCPTotalSize;
            TCPTransmissionTime = serverStats.TCPTransmissionTime;
            TCPTransmissionSpeed = serverStats.TCPTransmissionSpeed;
            TCPLostData = serverStats.TCPLostData;
            TCPTransmissionError = serverStats.TCPTransmissionError;
            UDPDataSize = serverStats.UDPDataSize;
            UDPTotalSize = serverStats.UDPTotalSize;
            UDPTransmissionTime = serverStats.UDPTransmissionTime;
            UDPTransmissionSpeed = serverStats.UDPTransmissionSpeed;
            UDPLostData = serverStats.UDPLostData;
            UDPTransmissionError = serverStats.UDPTransmissionError;
            recalculateStats("kb");
        }
        private void recalculateStats(String size)
        {
            switch(size)
            {
                case "b": break;
                case "kb":
                    {
                        divideBySize(1024);
                        break;
                    }
                case "mb":
                    {
                        divideBySize(1024 * 1024);
                        break;
                    }
                case "gb":
                    {
                        divideBySize(1024 * 1024 * 1024);
                        break;
                    }
            }
            if (Int32.Parse(TCPTransmissionTime) != 0)
            {
                TCPTransmissionSpeed = (Int32.Parse(TCPTotalSize) / Int32.Parse(TCPTransmissionTime)).ToString();
            }
            if (Int32.Parse(UDPTransmissionTime) != 0)
            {
                UDPTransmissionSpeed = (Int32.Parse(UDPTotalSize) / Int32.Parse(UDPTransmissionTime)).ToString();
            }
        }
        private void divideBySize(int size)
        {
            TCPTotalSize = (Int32.Parse(TCPTotalSize) / size).ToString();
            TCPTransmissionTime = (Int32.Parse(TCPTransmissionTime) / size).ToString();
            UDPTotalSize = (Int32.Parse(UDPTotalSize) / size).ToString();
            UDPTransmissionTime = (Int32.Parse(UDPTransmissionTime) / size).ToString();
        }
    }
}
