using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpeedTester.ViewModel
{
    class ClientMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand StartStopClient { get { return new RelayCommand(StartClients); } }
        public ICommand StartStopTCP { get { return new RelayCommand(StartClientTCP); } }
        public ICommand StartStopUDP { get { return new RelayCommand(StartClientUDP); } }
        private TCPClient tcpClient;
        private Thread tcpThread;
        private static IPAddress clientIPAddress;
        private static int clientPort;
        private static IPPortDelegate ipPortDelegate = setIpAndPort;
        public static IPPortDelegate IpPortDelegate { get => ipPortDelegate; set => ipPortDelegate = value; }
        public int bufferSize;
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
        private void StartClients()
        {
            
        }
        private void StartClientTCP()
        {
            tcpClient = new TCPClient(clientIPAddress, clientPort, BufferSize);
            tcpThread = new Thread(tcpClient.Run);
            tcpThread.Start();
        }
        private void StartClientUDP()
        {
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
    }
}
