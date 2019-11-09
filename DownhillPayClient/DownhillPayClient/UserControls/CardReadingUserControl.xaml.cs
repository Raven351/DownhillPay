using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using APIClient.Requests;
using ArduinoRFIDReader;
using DownhillPayClient.APIClient.Models;
using DownhillPayClient.APIClient.Requests;
using DownhillPayClient.Classes.Transactions;
using DownhillPayClient.MessageBoxLayout;
using RestSharp;

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
            rfidCardRequest = new RfidCardRequest();
            clientRequest = new ClientRequest();
            rfidCardSubscriptionRequest = new RfidCardSubscriptionRequest();
        }

        public MainWindow MainWindow { get; }
        public UserControl PreviousControl { get; set; }
        private readonly RfidCardRequest rfidCardRequest;
        private readonly ClientRequest clientRequest;
        private readonly RfidCardSubscriptionRequest rfidCardSubscriptionRequest;

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
                if (MainWindow.CardUid != null)
                {
                    try
                    {
                        if (typeof(NewCardTransaction) == MainWindow.Transaction.GetType())
                        {
                            var rfidCardsCount = rfidCardRequest.Get().Count();
                            MainWindow.Transaction.RfidCard.CardNumber = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + "/" + rfidCardsCount;
                            MainWindow.Transaction.RfidCard.Uid = MainWindow.CardUid.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                            MainWindow.Transaction.RfidCard.Uid2 = MainWindow.Transaction.RfidCard.Uid + rfidCardsCount; //TEMPORARY UID2
                            if (((NewCardTransaction)MainWindow.Transaction).IsPersonalized == true)
                            {
                                var clientCardRequestResponse = clientRequest.Post(((NewCardTransaction)MainWindow.Transaction).Client);
                                if (clientCardRequestResponse.StatusCode != HttpStatusCode.Created)
                                    throw new Exception("Error " + (int)clientCardRequestResponse.StatusCode + "\r" + clientCardRequestResponse.Content);
                                MainWindow.Transaction.RfidCard.ClientId = ((NewCardTransaction)MainWindow.Transaction).Client.Id;
                            }
                            var rfidCardRequestResponse = rfidCardRequest.Post(MainWindow.Transaction.RfidCard);
                            if (rfidCardRequestResponse.StatusCode != HttpStatusCode.Created)
                            {
                                if (rfidCardRequestResponse.StatusCode == HttpStatusCode.Conflict)
                                    throw new Exception("This card has already been registered!");
                                else throw new Exception("Error " + (int)rfidCardRequestResponse.StatusCode + "\r" + rfidCardRequestResponse.Content);

                            }
                            Debug.WriteLine((int)rfidCardRequestResponse.StatusCode);
                            Debug.WriteLine(MainWindow.Transaction.RfidCard.CardNumber);
                            Debug.WriteLine(rfidCardRequestResponse.Content);
                        }
                        var rfidCard = rfidCardRequest.Get(MainWindow.CardUid);
                        if (rfidCard == null) throw new NullReferenceException("Invalid card!");
                        else if (MainWindow.Transaction.TopUpType == TopUpTypes.Points)
                        {                          
                            var rfidCardRequestResponse = rfidCardRequest.PatchPoints(rfidCard.Uid, Convert.ToInt32(rfidCard.PointsBalance), MainWindow.Transaction.TopUpPoints);
                        }
                        #region Subscriptions Top Up
                        else if (MainWindow.Transaction.TopUpType == TopUpTypes.Subscription)
                        {
                            MainWindow.Transaction.RfidCardSubscription.IdRfidCard = rfidCard.Id; 
                            if (MainWindow.Transaction.RfidCardSubscription.DateStart == null || MainWindow.Transaction.RfidCardSubscription.DateStart < DateTime.Now)
                            {
                                MainWindow.Transaction.RfidCardSubscription.DateStart = DateTime.Now;
                                MainWindow.Transaction.RfidCardSubscription.DateEnd = DateTime.Now + MainWindow.Transaction.SubscriptionTimespan;
                            }
                            if (rfidCardSubscriptionRequest.Exists(MainWindow.CardUid, MainWindow.Transaction.RfidCardSubscription.DateStart, MainWindow.Transaction.RfidCardSubscription.DateEnd) == true)
                                throw new Exception("Subscription for given dates already exists for this card!");
                            var postResponse = rfidCardSubscriptionRequest.Post(MainWindow.Transaction.RfidCardSubscription);
                        } 
                        #endregion
                        messageBox.Message = "Payment started. Please take your card and pay at the cash payment point. Your card will be activated after payment is completed.";
                        MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
                        messageBox.ShowDialog();
                    }
                    catch (NullReferenceException e)
                    {
                        messageBox.Message = e.Message;
                        MainWindow.contentControl.Content = previousControl;
                        messageBox.ShowDialog();
                    }

                    catch (Exception e)
                    {
                        messageBox.Message = e.Message;
                        MainWindow.contentControl.Content = previousControl;
                        messageBox.ShowDialog();
                    }

                }
                else
                {
                    messageBox.Message = "Payment canceled. Try again.";
                    MainWindow.contentControl.Content = previousControl;
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
