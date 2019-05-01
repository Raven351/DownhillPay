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
using System.Management;
using System.Data.SqlClient;
using DownhillPayClient.UserControls;

namespace DownhillPayClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public POSMainMenuView POSMainMenuView { get; }
        public CheckPointsBalanceUserControl CheckPointsBalanceUserControl { get; }
        public MainWindow()
        {
            InitializeComponent();
            POSMainMenuView = new POSMainMenuView(this);
            CheckPointsBalanceUserControl = new CheckPointsBalanceUserControl(this);
            this.contentControl.Content = new POSMainMenuView(this);
            MessageBox.Show(Properties.Settings.Default.API_URI);
        }
    }
}
