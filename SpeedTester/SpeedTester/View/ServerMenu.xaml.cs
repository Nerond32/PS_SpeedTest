using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedTester
{
    public partial class ServerMenu : UserControl
    {
        bool running = false;
        public ServerMenu()
        {
            InitializeComponent();
        }

        void ChangeServerStatus(object sender, EventArgs e)
        {
            try
            {
                if (running)
                {
                    running = !running;
                    serverStatusButton.Content = "Start Server";
                }
                else
                {
                    running = true;
                    //IPAddress ipAddress = IPAddress.Parse(((MainWindow)Application.Current.MainWindow).ipTextBox.Text);
                    //int port = Int32.Parse(((MainWindow)Application.Current.MainWindow).portTextBox.Text);
                    serverStatusButton.Content = "Stop Server";
                }
            }
            catch
            {
                running = false;
                serverStatusButton.Content = "Start Server";
                MessageBox.Show("Invalid ip address or port entered!", "Input error");
            }
        }
    }
}
