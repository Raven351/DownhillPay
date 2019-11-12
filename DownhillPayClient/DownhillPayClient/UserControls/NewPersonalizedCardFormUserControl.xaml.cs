using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            RevertState();
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
            RevertState();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
            RevertState();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsDataProvidedCorrect() == false) MessageBox.Show("Data provided is incorrect!");
            else
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

                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
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

        private void RevertState()
        {
            ClearTextBoxes();
            FirstNameButton_Click(firstNameButton, e: null);
        }

        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            var key = ((TextBlock)((Viewbox)(sender as Button).Content).Child).Text;
            if(activeTextbox.Text.Length < activeTextbox.MaxLength) activeTextbox.AppendText(key);
        }

        private void PlKeyboard_Click(object sender, RoutedEventArgs e)
        {
            Keyboard_Click(sender, e);
            PlKeyboardBorder.Visibility = Visibility.Hidden;
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

        private void ChooseTextbox(object sender, TextBox previousActiveTextbox)
        {
            if (previousActiveTextbox != null) previousActiveTextbox.Background = Brushes.White;
            firstNameButton.Background = Brushes.White; //set default color
            lastNameButton.Background = Brushes.White;
            if (activeTextbox == firstNameTextBox || activeTextbox == lastNameTextBox)
            {
                NumKeyboardBorder.Visibility = Visibility.Hidden;
                NumKeyboard.IsEnabled = false;
                NumKeyboard.Visibility = Visibility.Hidden;
                KeyboardBorder.Visibility = Visibility.Visible;
                Keyboard.IsEnabled = true;
                Keyboard.Visibility = Visibility.Visible;
            }
            if (activeTextbox == phoneNumberTextBox || activeTextbox == dayTextBox || activeTextbox == monthTextBox || activeTextbox == yearTextBox)
            {
                KeyboardBorder.Visibility = Visibility.Hidden;
                Keyboard.IsEnabled = false;
                Keyboard.Visibility = Visibility.Hidden;
                PlKeyboardBorder.Visibility = Visibility.Hidden;
                NumKeyboardBorder.Visibility = Visibility.Visible;
                NumKeyboard.IsEnabled = true;
                NumKeyboard.Visibility = Visibility.Visible; 
            }
            (sender as Button).Background = Brushes.Yellow; //set selected color
            activeTextbox.Background = Brushes.Yellow;
            activeTextbox.Focus();
        }


        private void FirstNameButton_Click(object sender, RoutedEventArgs e)
        {
            var previousActiveTextbox = activeTextbox;
            activeTextbox = firstNameTextBox;
            ChooseTextbox(sender, previousActiveTextbox);
        }


        private void LastNameButton_Click(object sender, RoutedEventArgs e)
        {
            var previousActiveTextbox = activeTextbox;
            activeTextbox = lastNameTextBox;
            ChooseTextbox(sender, previousActiveTextbox);
        }

        private void PlKeysButton_Click(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsEnabled == true)
            {
                Keyboard.IsEnabled = false;
                PlKeyboardBorder.Visibility = Visibility.Visible;
                PlKeyboard.Visibility = Visibility.Visible;
                PlKeyboard.IsEnabled = true;
            }
            else
            {
                Keyboard.IsEnabled = true;
                PlKeyboardBorder.Visibility = Visibility.Hidden;
                PlKeyboard.Visibility = Visibility.Hidden;
                PlKeyboard.IsEnabled = false;
            }

        }

        private void PhoneNumberButton_Click(object sender, RoutedEventArgs e) //TODO: Optimize
        {
            var previousActiveTextbox = activeTextbox;
            activeTextbox = phoneNumberTextBox;
            ChooseTextbox(sender, previousActiveTextbox);
        }

        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            var previousActiveTextbox = activeTextbox;
            activeTextbox = dayTextBox;
            ChooseTextbox(sender, previousActiveTextbox);
        }

        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            var previousActiveTextbox = activeTextbox;
            activeTextbox = monthTextBox;
            ChooseTextbox(sender, previousActiveTextbox);
        }

        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            var previousActiveTextbox = activeTextbox;
            activeTextbox = yearTextBox;
            ChooseTextbox(sender, previousActiveTextbox);
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == firstNameTextBox) FirstNameButton_Click(firstNameButton, e);
            if (sender == lastNameTextBox) LastNameButton_Click(lastNameButton, e);
            if (sender == phoneNumberTextBox) PhoneNumberButton_Click(phoneNumberButton, e);
            if (sender == dayTextBox) DayButton_Click(dayButton, e);
            if (sender == monthTextBox) MonthButton_Click(monthButton, e);
            if (sender == yearTextBox) YearButton_Click(yearButton, e);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviewTextInputHandlerLetterKeyboard(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ\-\s]");
            if (regex.IsMatch(e.Text)) e.Handled = true;
        }

        private void PreviewTextInputHanderNumberKeyboard(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^0-9\s]");
            if (regex.IsMatch(e.Text)) e.Handled = true;
        }

        private bool IsDataProvidedCorrect()
        {
            IsFirstNameCorrect();
            IsLastNameCorrect();
            IsPhoneNumberCorrect();
            IsDayCorrect();
            IsMonthCorrect();
            IsYearCorrect();
            return IsFirstNameCorrect() != false && IsLastNameCorrect() != false && IsPhoneNumberCorrect() != false && IsDayCorrect() != false && IsMonthCorrect() != false && IsYearCorrect() != false;
        }

        private bool IsFirstNameCorrect()
        {
            if (firstNameTextBox.Text.Length < 2)
            {
                firstNameTextBox.Background = Brushes.IndianRed;
                return false;
            }
            else return true;
        }

        private bool IsLastNameCorrect()
        {
            if (lastNameTextBox.Text.Length < 2)
            {
                lastNameTextBox.Background = Brushes.IndianRed;
                return false;
            }
            else return true;
        }

        private bool IsPhoneNumberCorrect()
        {
            if (phoneNumberTextBox.Text.Length < 5)
            {
                phoneNumberTextBox.Background = Brushes.IndianRed;
                return false;
            }
            else return true;
        }

        private bool IsDayCorrect()
        {
            if (dayTextBox.Text.Length > 0)
            {
                if (dayTextBox.Text.Length > 2 || Convert.ToInt32(dayTextBox.Text) > 31 || Convert.ToInt32(dayTextBox.Text) < 1)
                {
                    dayTextBox.Background = Brushes.IndianRed;
                    return false;
                }
                else return true;
            }
            else
            {
                dayTextBox.Background = Brushes.IndianRed;
                return false;
            }
        }

        private bool IsMonthCorrect()
        {
            if (monthTextBox.Text.Length > 0)
            {
                if (monthTextBox.Text.Length > 2 || Convert.ToInt32(monthTextBox.Text) > 12 || Convert.ToInt32(monthTextBox.Text) < 1)
                {
                    monthTextBox.Background = Brushes.IndianRed;
                    return false;
                }
                else return true;
            }
            else
            {
                dayTextBox.Background = Brushes.IndianRed;
                return false;
            }
        }

        private bool IsYearCorrect()
        {
            if (yearTextBox.Text.Length > 0)
            {
                if (yearTextBox.Text.Length > 4 || Convert.ToInt32(yearTextBox.Text) > DateTime.Now.Year - 4 || Convert.ToInt32(yearTextBox.Text) < 1900)
                {
                    yearTextBox.Background = Brushes.IndianRed;
                    return false;
                }
                else return true;
            }
            else
            {
                dayTextBox.Background = Brushes.IndianRed;
                return false;
            }
        }
       
    }
}
