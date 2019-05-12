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
using APIClient.Models;
using APIClient.Requests;

namespace DownhillPayClient.UserControls
{
    /// <summary>
    /// Interaction logic for NewPersonalizedCardFormUserControl.xaml
    /// </summary>
    public partial class NewPersonalizedCardFormUserControl : UserControl, IUserControlWindow
    {
        public NewPersonalizedCardFormUserControl()
        {
            InitializeComponent();
        }

        public NewPersonalizedCardFormUserControl(MainWindow mainWindow) : this()
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
            MainWindow.Client = null;
            ClearTextBoxes();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
            MainWindow.Client = null;
            ClearTextBoxes();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Client = new Client(firstNameTextBox.Text, lastNameTextBox.Text, phoneNumberTextBox.Text,
                new DateTime(Convert.ToInt32(yearTextBox.Text), Convert.ToInt32(monthTextBox.Text), Convert.ToInt32(dayTextBox.Text))); //create client objects with provided data
            var clientRequest = new ClientRequest();
            var responseData = clientRequest.Get(firstNameTextBox.Text, lastNameTextBox.Text, phoneNumberTextBox.Text,
                new DateTime(Convert.ToInt32(yearTextBox.Text), Convert.ToInt32(monthTextBox.Text), Convert.ToInt32(dayTextBox.Text))); //get client from database that has the same data as provided
            if (responseData != null) //if data was returned, show MessageBox to client
            {
                MessageBox.Show("Client with provided data is already in database!", "Couldn't create new client");
            }
            else MainWindow.contentControl.Content = MainWindow.TopUpTypesUserControl.ChangeToControl(this); //else continue to next control 
            //var response = clientRequest.Post(MainWindow.Client); //post client to database
            //Debug.WriteLine(response);

        }

        private void ClearTextBoxes()
        {
            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
            phoneNumberTextBox.Clear();
            dayTextBox.Clear();
            monthTextBox.Clear();
            yearTextBox.Clear();
        }
    }
}
