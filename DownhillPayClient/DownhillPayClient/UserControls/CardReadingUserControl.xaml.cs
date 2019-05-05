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
    /// Interaction logic for CardReadingUserControl.xaml
    /// </summary>
    public partial class CardReadingUserControl : UserControl, IUserControlWindow
    {
        public CardReadingUserControl()
        {
            InitializeComponent();
        }

        public CardReadingUserControl(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }

        public UserControl PreviousControl { get; set; }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            return this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
        }
    }
}
