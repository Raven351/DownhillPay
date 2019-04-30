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
using DownhillPayClient.UserControls;

namespace DownhillPayClient
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        private readonly MainWindow parentWindow;
        public LoginUserControl()
        {
            InitializeComponent();
            //this.Loaded += LoginUserControl_Loaded;
        }

        public LoginUserControl(MainWindow mainWindow)
        {
            parentWindow = mainWindow;
            InitializeComponent();
        }

        private void StaffLoginButton_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.contentControl.Content = new POSMainMenuView();
        }
    }
}
