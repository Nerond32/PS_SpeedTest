using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace SpeedTester
{
    public partial class ClientMenu : UserControl
    {
        bool running;
        public ClientMenu()
        {
            InitializeComponent();
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
                //ipAddress = IPAddress.Parse(((MainWindow)Application.Current.MainWindow).ipTextBox.Text);
                //port = Int32.Parse(((MainWindow)Application.Current.MainWindow).portTextBox.Text);
                running = true;
                clientConnectButton.Content = "Disconnect";
            }
            catch
            {
                running = false;
                clientConnectButton.Content = "Connect";
                MessageBox.Show("Invalid ip address or port entered!", "Input error");
            }
        }
    }
}
