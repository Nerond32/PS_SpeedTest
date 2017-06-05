using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows;

namespace SpeedTester
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata
            {
                DefaultValue = FindResource(typeof(Window))
            });
            Closing += OnWindowClosing;
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
