using SpeedTester.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpeedTester.ViewModel
{
    class ClientMenuViewModel : INotifyPropertyChanged
    {
        private static IPPortDelegate ipPortDelegate = setIpAndPort;
        private TCPClient tcpClient;
        private UDPClient udpClient;
        private Thread tcpThread, udpThread;
        private bool isRunning;
        private static IPAddress clientIPAddress = IPAddress.Parse("127.0.0.1");
        private static int clientPort = 7;
        private string startStopClientText = "Start";
        private string tcpConnectionText = "TCP: Disconnected";
        private string udpStatusText = "UDP: Stopped";
        public static IPPortDelegate IpPortDelegate { get => ipPortDelegate; set => ipPortDelegate = value; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand StartStopClient { get { return new RelayCommand(StartClients); } }
        private int bufferSize;
        #region Properties
        public int BufferSize
        {
            get
            {
                return bufferSize;
            }
            set
            {
                bufferSize = value;
            }
        }
        public string StartStopClientText
        {
            get
            {
                return startStopClientText;
            }
            set
            {
                startStopClientText = value;
                OnPropertyChanged("StartStopClientText");
            }
        }
        
        public string TCPConnectionText
        {
            get
            {
                return tcpConnectionText;
            }
            set
            {
                tcpConnectionText = value;
                OnPropertyChanged("TCPConnectionText");
            }
        }
        public string UDPStatusText
        {
            get
            {
                return udpStatusText;
            }
            set
            {
                udpStatusText = value;
                OnPropertyChanged("UDPStatusText");
            }
        }
        #endregion
        private void StartClients()
        {
            if (isRunning)
            {
                StopTCP();
                StopUDP();
                isRunning = false;
                StartStopClientText = "Start";
            }
            else
            {
                StartClientTCP();
                StartClientUDP();
                isRunning = true;
                StartStopClientText = "Stop";
            }
        }
        private void StartClientTCP()
        {
            try
            {
                tcpClient = new TCPClient(clientIPAddress, clientPort, BufferSize);
                tcpClient.OnBrokenConnection += BrokenTCPConnection;
                tcpThread = new Thread(tcpClient.Run);
                tcpThread.Start();
                TCPConnectionText = "TCP: Connected";
            }
            catch(SocketException e) { }
        }
        private void StopTCP()
        {
            try
            {
                tcpClient.RequestStop();
                TCPConnectionText = "TCP: Disconnected";
            }
            catch (NullReferenceException) { }
        }
        private void StartClientUDP()
        {
            udpClient = new UDPClient(clientIPAddress, clientPort, BufferSize);
            udpThread = new Thread(udpClient.Run);
            udpThread.Start();
            UDPStatusText = "UDP: Working";
        }
        private void StopUDP()
        {

            UDPStatusText = "UDP: Stopped";
        }
        private static void setIpAndPort(string ipAddress, int port)
        {
            try
            {
                IPAddress tmpAddress = IPAddress.Parse(ipAddress);
                int tmpPort = port;
                clientIPAddress = tmpAddress;
                clientPort = tmpPort;
            }
            catch { }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void BrokenTCPConnection()
        {
            TCPConnectionText = "TCP: Disconnected";
        }
    }
}
