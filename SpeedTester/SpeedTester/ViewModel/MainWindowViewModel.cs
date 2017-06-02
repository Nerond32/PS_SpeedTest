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
        public event PropertyChangedEventHandler PropertyChanged;
        public String IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 7;
        private String selectedMode;
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
        private Visibility clientVisibility = Visibility.Visible, serverVisibility = Visibility.Hidden;
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
