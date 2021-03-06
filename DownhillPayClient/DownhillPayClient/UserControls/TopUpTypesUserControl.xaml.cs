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
    /// Interaction logic for TopUpTypesUserControl.xaml
    /// </summary>
    public partial class TopUpTypesUserControl : UserControl, IUserControlWindow
    {
        public TopUpTypesUserControl()
        {
            InitializeComponent();
        }

        public TopUpTypesUserControl(MainWindow mainWindow) : this()
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

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = PreviousControl;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
        }

        private void PointsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.PointTopUpUserControl.ChangeToControl(this);
        }

        private void SubscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.contentControl.Content = MainWindow.SubscriptionTopUpUserControl.ChangeToControl(this);
        }
    }
}
