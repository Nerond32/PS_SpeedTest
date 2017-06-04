using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpeedTester.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private String selectedMode;
        private String ipAddress = "127.0.0.1";
        private int port = 7;
        private Visibility clientVisibility = Visibility.Visible;
        private Visibility serverVisibility = Visibility.Hidden;
        public event PropertyChangedEventHandler PropertyChanged;
        #region Properties
        public String IPAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
                OnPropertyChanged("IPAddress");
                ServerMenuViewModel.IpPortDelegate(IPAddress, Port);
                ClientMenuViewModel.IpPortDelegate(IPAddress, Port);
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                OnPropertyChanged("Port");
                ServerMenuViewModel.IpPortDelegate(IPAddress, Port);
                ClientMenuViewModel.IpPortDelegate(IPAddress, Port);
            }
        }
        public String SelectedMode
        {
            get
            {
                return selectedMode;
            }
            set
            {
                selectedMode = value;
                ChangeMode();
            }
        }
        public Visibility ClientVisibility
        {
            get { return clientVisibility; }
            set
            {
                clientVisibility = value;
                OnPropertyChanged("ClientVisibility");
            }
        }
        public Visibility ServerVisibility
        {
            get { return serverVisibility; }
            set
            {
                serverVisibility = value;
                OnPropertyChanged("ServerVisibility");
            }
        }
        #endregion
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ChangeMode()
        {
            ClientVisibility = Visibility.Hidden;
            ServerVisibility = Visibility.Hidden;
            if (SelectedMode == "System.Windows.Controls.ComboBoxItem: Server")
            {
                ServerVisibility = Visibility.Visible;
                IPAddress = "127.0.0.1";
            }
            if(SelectedMode == "System.Windows.Controls.ComboBoxItem: Client")
            {
                ClientVisibility = Visibility.Visible;
            }
        }
    }
}
