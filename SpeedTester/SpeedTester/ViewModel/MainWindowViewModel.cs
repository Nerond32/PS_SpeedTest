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
        //public ICommand ChangeMode { get { return new RelayCommand(ExecuteChangeMode); } }
        public String IPAddress { get; set; }
        private String selectedMode;
        public int Port { get; set; }
        private Visibility clientVisibility = Visibility.Visible, serverVisibility = Visibility.Visible;
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
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        private void ChangeMode()
        {
            Console.WriteLine(SelectedMode);
            //clientVisibility = Visibility.Hidden;
            //serverVisibility = Visibility.Hidden;
            if (SelectedMode == "System.Windows.Controls.ComboBoxItem: Server")
            {
                Console.WriteLine("XXX");
                serverVisibility = Visibility.Visible;
                IPAddress = "127.0.0.1";
            }
            if(SelectedMode == "System.Windows.Controls.ComboBoxItem: Client")
            {
                Console.WriteLine("YYY");
                clientVisibility = Visibility.Visible;
            }
        }
    }
}
