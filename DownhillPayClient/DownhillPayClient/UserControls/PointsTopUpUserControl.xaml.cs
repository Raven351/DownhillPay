using DownhillPayClient.APIClient.Models;
using DownhillPayClient.APIClient.Requests;
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
    /// Interaction logic for PointTopUpUserControl.xaml
    /// </summary>
    public partial class PointTopUpUserControl : UserControl, IUserControlWindow
    {
        public PointTopUpUserControl()
        {
            InitializeComponent();
            InitializePointsButtons();
        }

        public PointTopUpUserControl(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }

        public UserControl PreviousControl { get; set; }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            if (PreviousControl == MainWindow.POSMainMenuView) CancelButton.Visibility = Visibility.Hidden;
            else CancelButton.Visibility = Visibility.Visible;
            return this;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
        }

        private void Value1_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Transaction.PaymentValue += 450;
            MainWindow.Transaction.TopUpPoints += 45;
            MainWindow.Transaction.RfidCard.PointsBalance += 45;
            MainWindow.contentControl.Content = MainWindow.PaymentMethodUserControl.ChangeToControl(this);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PointsClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Transaction.TopUpType = Classes.Transactions.TopUpTypes.Points;
            MainWindow.Transaction.TopUpPoints = ((Points)((Button)sender).Tag).Amount;
            MainWindow.Transaction.PaymentValue = ((Points)((Button)sender).Tag).Price;
            MainWindow.contentControl.Content = MainWindow.PaymentMethodUserControl.ChangeToControl(this);
        }

        private void InitializePointsButtons()
        {
            var buttons = PointsTopUpButtonsRow1.Children.OfType<Button>().Concat(PointsTopUpButtonsRow2.Children.OfType<Button>());
            var pointsCollection = new PointsRequest().GetByPosNumber(notNull: true);
            foreach (var button in buttons.Where(button => button.Tag != null))
            {
                button.Tag = pointsCollection.Where(points => points.PosNumber == Convert.ToInt32(button.Tag.ToString())).FirstOrDefault();
                if (button.Tag == null) button.IsEnabled = false;
                else ((TextBlock)button.Content).Text = ((Points)button.Tag).Amount + " points\n" + ((decimal)((Points)button.Tag).Price / 100).ToString("0") + " PLN";
            }
        }
    }
}
