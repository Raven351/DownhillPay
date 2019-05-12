﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using ArduinoRFIDReader;

namespace DownhillPayClient.UserControls
{
    /// <summary>
    /// Interaction logic for CardReadingUserControl.xaml
    /// </summary>
    public partial class CardReadingUserControl : UserControl, IUserControlWindow //INotifyPropertyChanged
    {
        public CardReadingUserControl()
        {
            InitializeComponent();
        }

        public CardReadingUserControl(MainWindow mainWindow) : this()
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }

        public UserControl PreviousControl { get; set; }

        public UserControl ChangeToControl(UserControl previousControl)
        {
            PreviousControl = previousControl;
            return this;
        }

        public UserControl ChangeToControl(UserControl previousControl, string message)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(message);
            messageFlowDocument.Blocks.Clear();
            messageFlowDocument.Blocks.Add(paragraph);
            return ChangeToControl(previousControl);
        }

        //public async Task<UserControl> ChangeToControlAsync(UserControl previousControl)
        //{
        //    var changeToControlTask = Task.Run(() => ChangeToControl(previousControl));
        //    var readCardTask = ReadCardAsync();
        //    await readCardTask;
        //    return await changeToControlTask;
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MFRC522ReaderWriter.Close();
            MainWindow.contentControl.Content = PreviousControl;
        }

        public async Task ReadCardAsync() //not used
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            string uid = await MainWindow.MFRC522ReaderWriter.ReadUIDAsync();
            Debug.WriteLine(uid);
        }
    }
}