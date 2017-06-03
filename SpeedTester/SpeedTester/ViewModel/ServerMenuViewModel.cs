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
        enum Sizes {
            KB = 1024,
            MB = 1024 * 1024,
            GB = 1024 * 1024 * 1024
        }
        private static IPPortDelegate ipPortDelegate = setIpAndPort;
        private Thread tcpThread;
        private Thread udpThread;
        private Server tcpServer, udpServer;
        private bool running = false;
        private static IPAddress serverIPAddress;
        private static int serverPort;
        #region Binding Strings
        private string serverStatusButtonText = "Start server";
        private String tcpDataSize = "0";
        private String tcpTotalSize = "0";
        private String tcpTransmissionTime = "0";
        private String tcpTransmissionSpeed = "0";
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
                    udpServer.RequestStop();
                    ServerStatusButtonText = "Start Server";
                }
                else
                {
                    ResetStats();
                    running = true;
                    tcpServer = new TCPServer(serverIPAddress, serverPort);
                    tcpServer.OnStatsUpdate += tcpStatsUpdate;
                    tcpThread = new Thread(tcpServer.Run);
                    tcpThread.Start();
                    udpServer = new UDPServer(serverIPAddress, serverPort);
                    udpServer.OnStatsUpdate += udpStatsUpdate;
                    udpThread = new Thread(udpServer.Run);
                    udpThread.Start();
                    ServerStatusButtonText = "Stop Server";
                }
            }
            catch
            {
                running = false;
                ServerStatusButtonText = "Start Server";
                MessageBox.Show("Error while starting server occured!", "Input error");
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
        #region StatsManagement
        private void tcpStatsUpdate(ServerStats serverStats)
        {
            recalculateStats(serverStats, (int)Sizes.KB);
            TCPDataSize = serverStats.DataSize.ToString();
            TCPTotalSize = serverStats.TotalSize.ToString();
            TCPTransmissionTime = serverStats.TransmissionTime.ToString();
            TCPTransmissionSpeed = serverStats.TransmissionSpeed.ToString();
        }
        private void udpStatsUpdate(ServerStats serverStats)
        {
            recalculateStats(serverStats, (int)Sizes.KB);
            UDPDataSize = serverStats.DataSize.ToString();
            UDPTotalSize = serverStats.TotalSize.ToString();
            UDPTransmissionTime = serverStats.TransmissionTime.ToString();
            UDPTransmissionSpeed = serverStats.TransmissionSpeed.ToString();
            UDPLostData = serverStats.LostData.ToString();
            UDPTransmissionError = serverStats.TransmissionError.ToString();
        }
        private void ResetStats()
        {
            TCPDataSize = "0";
            TCPTotalSize = "0";
            TCPTransmissionTime = "0";
            TCPTransmissionSpeed = "0";
            UDPDataSize = "0";
            UDPTotalSize = "0";
            UDPTransmissionTime = "0";
            UDPTransmissionSpeed = "0";
            UDPLostData = "0";
            UDPTransmissionError = "0";
        }
        private void recalculateStats(ServerStats serverStats, int size)
        {
            if (serverStats.TransmissionTime != 0)
            {
                int speed = (int)((float)serverStats.TotalSize  / ((float)serverStats.TransmissionTime / 1000));
                serverStats.TransmissionSpeed = speed;
            }
            if (size > 0)
            {
                serverStats.TotalSize /= size;
                serverStats.TransmissionSpeed /= size;
            }
        }
        #endregion
    }
}
