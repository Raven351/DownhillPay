using System;
using System.Collections.Generic;
using System.Linq;
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
using ArduinoRFIDReader;

namespace DownhillPayClient
{
    /// <summary>
    /// Interaction logic for AppSettingsUserControl.xaml
    /// </summary>
    public partial class AppSettingsUserControl : UserControl
    {
        public MainWindow MainWindow { get; }
        public AppSettingsUserControl()
        {
            InitializeComponent();
        }

        public AppSettingsUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void AutodetectRFIDPortButton_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ArduinoPortDetection.AutodetectPort() == null) MessageBox.Show("Arduino device hasn't been found","ERROR");
                else rfidPortTextBox.Text = ArduinoPortDetection.AutodetectPort();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        }
    }
}
