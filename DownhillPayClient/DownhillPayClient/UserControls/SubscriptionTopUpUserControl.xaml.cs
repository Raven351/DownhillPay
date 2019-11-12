using DownhillPayClient.APIClient.Models;
using DownhillPayClient.APIClient.Requests;
using DownhillPayClient.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SubscriptionTopUpUserControl : UserControl, IUserControlWindow
    {
        public SubscriptionTopUpUserControl()
        {
            InitializeComponent();
            InitializeSubscriptionButtons();
        }

        public SubscriptionTopUpUserControl(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }

        public UserControl PreviousControl { get ; set ; }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            if (PreviousControl == MainWindow.POSMainMenuView) CancelButton.Visibility = Visibility.Hidden;
            else CancelButton.Visibility = Visibility.Visible;
            return this;
        }

        #region ButtonClicks
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");
        }
        private void Subscription_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Transaction.TopUpType = Classes.Transactions.TopUpTypes.Subscription;
            MainWindow.Transaction.SubscriptionTimespan = ((Subscription)((Button)sender).Tag).Period;
            MainWindow.Transaction.PaymentValue = ((Subscription)((Button)sender).Tag).Price;
            if (((Subscription)((Button)sender).Tag).StartTime > new DateTime(year:0001, 1, 1)) //TODO: START DATE CORRECT
            {
                MainWindow.Transaction.RfidCardSubscription.DateStart = ((Subscription)((Button)sender).Tag).StartTime; 
                MainWindow.Transaction.RfidCardSubscription.DateEnd = ((Subscription)((Button)sender).Tag).EndTime;
                if (MainWindow.Transaction.RfidCardSubscription.DateEnd - TimeSpan.FromHours(1) <= DateTime.Now) MessageBox.Show("Cannot buy this subscription for today anymore!");
                else MainWindow.contentControl.Content = MainWindow.PaymentMethodUserControl.ChangeToControl(this);
            }       
            else MainWindow.contentControl.Content = MainWindow.PaymentMethodUserControl.ChangeToControl(this);
        }
        #endregion

        /// <summary>
        /// Assigns subscription object based on data from database to each Subscription button based on initial Button.Tag property and PosNumber endpoint in API.
        /// </summary>
        private void InitializeSubscriptionButtons()
        {
            //START - All buttons have Tag property with their consecutive number
            var buttons = HoursTopUpsButtons.Children.OfType<Button>().Concat(DaysTopUpsButtons.Children.OfType<Button>()); 
            var subscriptionCollection = new SubscriptionRequest().GetByPosNumber(notNull: true);
            foreach (var button in buttons.Where(button => button.Tag != null)) 
            {
                button.Tag = subscriptionCollection.Where(subscription => subscription.PosNumber == Convert.ToInt32(button.Tag.ToString())).FirstOrDefault();       
                if (button.Tag == null) button.IsEnabled = false; 
                else ((TextBlock)button.Content).Text = ((Subscription)button.Tag).Name + "\n" + ((decimal)((Subscription)button.Tag).Price / 100).ToString("0.00") + " PLN"; 
            }
        }


    }
}
