﻿using System;
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
    /// Interaction logic for PaymentMethod.xaml
    /// </summary>
    public partial class PaymentMethod : UserControl, IUserControlWindow
    {
        public PaymentMethod()
        {
            InitializeComponent();
        }

        public PaymentMethod(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }

        public UserControl PreviousControl { get; set; }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            AmountLabel.Content = ((decimal)MainWindow.PaymentValue / 100).ToString("0.00") + " PLN";
            return this;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.PaymentValue = 0;
            MainWindow.contentControl.Content = PreviousControl;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
        }
    }
}
