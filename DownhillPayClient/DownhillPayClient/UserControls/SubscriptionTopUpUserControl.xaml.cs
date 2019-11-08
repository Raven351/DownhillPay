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
            if (((Subscription)((Button)sender).Tag).StartDate != null) MainWindow.Transaction.RfidCardSubscription.DateStart = ((Subscription)((Button)sender).Tag).StartDate;
            MainWindow.contentControl.Content = MainWindow.PaymentMethodUserControl;
        }
        #endregion

        /// <summary>
        /// Assigns subscription object based on data from database to each Subscription button based on initial Button.Tag property and PosNumber endpoint in API.
        /// </summary>
        private void InitializeSubscriptionButtons()
        {
            //For optimization - could be done with one request to list TODO 
            //START - All buttons have Tag property with their consecutive number
            var buttons = HoursTopUpsButtons.Children.OfType<Button>().Concat(DaysTopUpsButtons.Children.OfType<Button>()); //Concat collection (grids) of buttons (red and green background in UI)
            foreach (var button in buttons.Where(button => button.Tag != null)) //For every button in collection which Tag property is not empty...
            {
                var subcribtionRequest = new SubscriptionRequest(); //Create API Request object.
                button.Tag = subcribtionRequest.Get(Convert.ToInt32(button.Tag.ToString())); //Execute GET request for Subscription object which PosNumber == Button.Tag and overwrite Button.Tag with returned object
                if (button.Tag == null) button.IsEnabled = false; //If button's tag was overwritten with null, that means there is no corresponding subscription in DB for this button, so it gets disabled.
                else ((TextBlock)button.Content).Text = ((Subscription)button.Tag).Name + "\n" + ((decimal)((Subscription)button.Tag).Price / 100).ToString("0") + " PLN"; //Else show subscription's data as button's textblock
            }
        }


    }
}
