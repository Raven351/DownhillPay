﻿using System;
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
using DownhillPayClient.Classes.Transactions;

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
        private TextBox activeTextbox;

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
            ClearTextBoxes();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
            ClearTextBoxes();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((NewCardTransaction)MainWindow.Transaction).Client = new Client(firstNameTextBox.Text, lastNameTextBox.Text, phoneNumberTextBox.Text,
                    new DateTime(Convert.ToInt32(yearTextBox.Text), Convert.ToInt32(monthTextBox.Text), Convert.ToInt32(dayTextBox.Text))); //create client objects with provided data
                var clientRequest = new ClientRequest();
                var responseData = clientRequest.Get(firstNameTextBox.Text, lastNameTextBox.Text, phoneNumberTextBox.Text,
                    new DateTime(Convert.ToInt32(yearTextBox.Text), Convert.ToInt32(monthTextBox.Text), Convert.ToInt32(dayTextBox.Text))); //get client from database that has the same data as provided
                if (responseData != null) //if data was returned, show MessageBox to client
                {
                    MessageBox.Show("Client with provided data is already in database!", "Couldn't create new client");
                }
                else MainWindow.contentControl.Content = MainWindow.TopUpTypesUserControl.ChangeToControl(this); 
                //else continue to next control                                                                                                
                //var response = clientRequest.Post(MainWindow.Client); //post client to database                                                                                                
                //Debug.WriteLine(response);
            }

            catch (FormatException)
            {
                MessageBox.Show("Invalid format!");
            }

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

        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            var key = ((TextBlock)((Viewbox)(sender as Button).Content).Child).Text;
            activeTextbox.AppendText(key);
        }

        private void PlKeyboard_Click(object sender, RoutedEventArgs e)
        {
            Keyboard_Click(sender, e);
            PlKeyboard.Visibility = Visibility.Hidden;
            PlKeyboard.IsEnabled = false;
            Keyboard.IsEnabled = true;
        }

        private void KeyboardBackspace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                activeTextbox.Text = activeTextbox.Text.Substring(0, activeTextbox.Text.Length - 1);
            }
            catch (ArgumentOutOfRangeException)
            {

            }         
        }

        private void ChooseTextbox(object sender)
        {

            firstNameButton.Background = Brushes.White; //set default color
            if (activeTextbox == firstNameTextBox || activeTextbox == lastNameTextBox)
            {
                Keyboard.IsEnabled = true;
                Keyboard.Visibility = Visibility.Visible;
            }
            (sender as Button).Background = Brushes.Yellow; //set selected color
        }


        private void FirstNameButton_Click(object sender, RoutedEventArgs e) //TODO
        {
            activeTextbox = firstNameTextBox;
            ChooseTextbox(sender);
        }


        private void LastNameButton_Click(object sender, RoutedEventArgs e)
        {            
            activeTextbox = lastNameTextBox;
            ChooseTextbox(sender);
        }

        private void PlKeysButton_Click(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsEnabled == true)
            {
                Keyboard.IsEnabled = false;
                PlKeyboard.Visibility = Visibility.Visible;
                PlKeyboard.IsEnabled = true;
            }
            else
            {
                Keyboard.IsEnabled = true;
                PlKeyboard.Visibility = Visibility.Hidden;
                PlKeyboard.IsEnabled = false;
            }

        }
    }
}
