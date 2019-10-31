using System;
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
using DownhillPayClient.MessageBoxLayout;

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

        public async void ChangeToControlVoid(UserControl previousControl, string message)
        {
            MessageBoxLayoutInfo messageBox = new MessageBoxLayoutInfo();
            MainWindow.contentControl.Content = ChangeToControl(previousControl, message);
            if (previousControl == MainWindow.POSMainMenuView)
            {
                MainWindow.CardUid = await MainWindow.MFRC522ReaderWriter.ReadUIDAsync();
                Debug.WriteLine(MainWindow.CardUid);
                MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
            }

            if (previousControl == MainWindow.PaymentMethodUserControl)
            {
                MainWindow.contentControl.Content = MainWindow.CardReadingUserControl.ChangeToControl(this, message);
                MainWindow.CardUid = await MainWindow.MFRC522ReaderWriter.ReadUIDAsync();
                if(MainWindow.CardUid == null)
                {
                    messageBox.Message = "Payment canceled. Try again.";
                    MainWindow.contentControl.Content = previousControl;
                    messageBox.ShowDialog();
                }
                else
                {
                    messageBox.Message = "Payment started. Please take your card and pay at the cash payment point. Your card will be activated after payment is completed.";
                    MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
                    messageBox.ShowDialog();
                }
            }
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
