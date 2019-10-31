using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for POSMainMenuView.xaml
    /// </summary>
    public partial class POSMainMenuView : UserControl
    {
        public MainWindow MainWindow { get; }
        public POSMainMenuView()
        {
            InitializeComponent();
        }

        public POSMainMenuView(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        private async void CheckPointsBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            //var task = MainWindow.CardReadingUserControl.ReadCardAsync();
            //MainWindow.contentControl.Content = MainWindow.CardReadingUserControl.ChangeToControl(this);
            //await task;
            //MainWindow.contentControl.Content = MainWindow.POSMainMenuView;

            MainWindow.CardReadingUserControl.ChangeToControlVoid(this, "Please place your card close to the reader.");

        }

        private void NewCardButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.NewCardUserControl.ChangeToControl(this);
            MainWindow.Transaction = new Classes.Transactions.NewCardTransaction();
            MainWindow.Transaction.RfidCard = new APIClient.Models.RfidCard();
        }

        private void TopUpCardButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.TopUpTypesUserControl.ChangeToControl(this);
            MainWindow.Transaction = new Classes.Transactions.Transaction();
        }
    }
}
