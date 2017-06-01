using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows;

namespace SpeedTester
{
    public partial class MainWindow : Window
    {
        private bool running = false;
        Thread tcpServerThread, tcpClientThread;
        ServerTCP tcpS;
        ClientTCP tcpC;
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow.OnWindowClosing;
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });
        }

        private static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        void ChangeVisibility(object sender, EventArgs e)
        {
            clientGrid.Visibility = Visibility.Hidden;
            serverGrid.Visibility = Visibility.Hidden;
            if(serverOrClientCB.Text == "Server")
            {
                serverGrid.Visibility = Visibility.Visible;
            }
            if(serverOrClientCB.Text == "Client")
            {
                clientGrid.Visibility = Visibility.Visible;
            }
        }

        void ChangeServerStatus(object sender, EventArgs e)
        {
            if(running == true)
            {
                serverStatusButton.Content = "Start server"; 
                running = false;
                tcpS.RequestStop();
                //tcpThread.Abort();
                return;
            }
            IPAddress ipAddress;
            int port;
            try
            {
                ipAddress = IPAddress.Parse(ipTextBox.Text);
                port = Int32.Parse(portTextBox.Text);
                running = true;
                serverStatusButton.Content = "Stop server";
                tcpS = new ServerTCP(ipAddress, port);
                tcpServerThread = new Thread(tcpS.Run);
                tcpServerThread.Start();
            }
            catch
            {
                running = false;
                MessageBox.Show("Invalid ip address or port entered!", "Input error");
            }
        }

        private void ClientConnect(object sender, RoutedEventArgs e)
        {
            IPAddress ipAddress;
            int port;
            if (running == true)
            {
                clientConnectButton.Content = "Connect";
                running = false;
                return;
            }
            try
            {
                ipAddress = IPAddress.Parse(ipTextBox.Text);
                port = Int32.Parse(portTextBox.Text);
                running = true;
                clientConnectButton.Content = "Disconnect";
                tcpC = new ClientTCP(ipAddress, port);
                tcpClientThread = new Thread(tcpC.Run);
                tcpClientThread.Start();
            }
            catch
            {
                running = false;
                MessageBox.Show("Invalid ip address or port entered!", "Input error");
            }
        }
        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
