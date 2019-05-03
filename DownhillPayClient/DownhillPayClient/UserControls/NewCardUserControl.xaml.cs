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
    /// Interaction logic for NewCardUserControl.xaml
    /// </summary>
    public partial class NewCardUserControl : UserControl, IUserControlWindow
    {
        public MainWindow MainWindow { get; }

        public UserControl PreviousControl { get ; set ; }

        public NewCardUserControl()
        {
            InitializeComponent();
        }

        public NewCardUserControl(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            return this;
        }

        private void PersonalizedCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
        }

        private void NonPersonalizedCardButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.TopUpTypesUserControl.ChangeToControl(this);
        }
    }
}
