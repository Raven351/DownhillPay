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

namespace DownhillPayClient.UserControls
{
    /// <summary>
    /// Interaction logic for CheckPointsBalanceUserControl.xaml
    /// </summary>
    public partial class CheckPointsBalanceUserControl : UserControl, IUserControlWindow
    {
        public MainWindow MainWindow { get; }
        public UserControl PreviousControl { get; set; }
        public CheckPointsBalanceUserControl()
        {
            InitializeComponent();
        }

        public CheckPointsBalanceUserControl(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            return this;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
        }

    }
}
