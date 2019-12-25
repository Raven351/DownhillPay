using APIClient.Requests;
using DownhillPayClient.APIClient.Models;
using DownhillPayClient.APIClient.Requests;
using DownhillPayClient.Classes.MessageBoxes;
using DownhillPayClient.Classes.Transactions;
using DownhillPayClient.MessageBoxLayout;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DownhillPayClient.UserControls
{
    /// <summary>
    /// Interaction logic for CardReadingUserControl.xaml
    /// </summary>
    public partial class CardReadingUserControl : UserControl, IUserControlWindow //INotifyPropertyChanged
    {
        private readonly ClientRequest clientRequest;

        private readonly RfidCardRequest rfidCardRequest;

        private readonly RfidCardSubscriptionRequest rfidCardSubscriptionRequest;

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
            try
            {
                MainWindow.CardUid = await MainWindow.MFRC522ReaderWriter.ReadUIDAsync();
                #region Checking card balance
                if (previousControl == MainWindow.POSMainMenuView)
                {
                    CheckCardBalance();
                }
                #endregion Checking card balance
                if (previousControl == MainWindow.PaymentMethodUserControl)
                {
                    MainWindow.contentControl.Content = MainWindow.CardReadingUserControl.ChangeToControl(this, message);
                    if (MainWindow.CardUid != null)
                    {
                        ExecuteTransaction();
                        messageBox.Message = "Payment started. Please take your card and pay at the cash payment point. Your card will be activated after payment is completed.";
                        MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
                        messageBox.ShowDialog();
                    }
                    else
                    {
                        messageBox.Message = "PAYMENT CANCELED\n TRY AGAIN";
                        messageBox.MessageBoxContent.Background = new SolidColorBrush(Color.FromRgb(211, 105, 105));
                        MainWindow.contentControl.Content = previousControl;
                        messageBox.ShowDialog();
                    }
                }
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

        private void AddNewCard()
        {
            if (rfidCardRequest.Get(MainWindow.CardUid) != null) throw new Exception("This card is already registered!");
            var rfidCardsCount = rfidCardRequest.Get().Count();
            MainWindow.Transaction.RfidCard.CardNumber = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + "/" + rfidCardsCount;
            MainWindow.Transaction.RfidCard.Uid = MainWindow.CardUid.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            MainWindow.Transaction.RfidCard.Uid2 = MainWindow.Transaction.RfidCard.Uid + rfidCardsCount; //TEMPORARY UID2
            var rfidCardRequestResponse = rfidCardRequest.Post(MainWindow.Transaction.RfidCard);
            if (rfidCardRequestResponse.StatusCode != HttpStatusCode.Created)
            {
                if (rfidCardRequestResponse.StatusCode == HttpStatusCode.Conflict)
                    throw new Exception("This card has already been registered!");
                else throw new Exception("Error " + (int)rfidCardRequestResponse.StatusCode + "\r" + rfidCardRequestResponse.Content);
            }
            if (((NewCardTransaction)MainWindow.Transaction).IsPersonalized == true)
            {
                AddNewClient();
            }
            Debug.WriteLine((int)rfidCardRequestResponse.StatusCode);
            Debug.WriteLine(MainWindow.Transaction.RfidCard.CardNumber);
            Debug.WriteLine(rfidCardRequestResponse.Content);
        }

        private void AddNewClient()
        {
            var clientCardRequestResponse = clientRequest.Post(((NewCardTransaction)MainWindow.Transaction).Client);
            if (clientCardRequestResponse.StatusCode != HttpStatusCode.Created)
                throw new Exception("Error " + (int)clientCardRequestResponse.StatusCode + "\r" + clientCardRequestResponse.Content);
            MainWindow.Transaction.RfidCard.ClientId = ((NewCardTransaction)MainWindow.Transaction).Client.Id;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MFRC522ReaderWriter.Close();
            MainWindow.contentControl.Content = PreviousControl;
        }

        private void CheckCardBalance()
        {
            MessageBoxLayoutInfo messageBox = new MessageBoxLayoutInfo();
            if (MainWindow.CardUid != null)
            {
                var card = rfidCardRequest.Get(MainWindow.CardUid);
                if (card == null) throw new NullReferenceException("Invalid card!");
                else
                {
                    var cardInfo = new CardInfo(card);
                    messageBox = cardInfo.GetMessageBox();
                }
            }
            else messageBox.Message = "Operation canceled!";

            MainWindow.contentControl.Content = MainWindow.POSMainMenuView;
            messageBox.ShowDialog();
        }

        private void ExecuteTransaction()
        {
            #region New card

            if (typeof(NewCardTransaction) == MainWindow.Transaction.GetType())
            {
                AddNewCard();
            }

            #endregion New card

            var rfidCard = rfidCardRequest.Get(MainWindow.CardUid);
            if (rfidCard == null) throw new NullReferenceException("Invalid card!");

            #region Points Top Up

            else if (MainWindow.Transaction.TopUpType == TopUpTypes.Points)
            {
                TopUpPoints(rfidCard);
            }

            #endregion Points Top Up

            #region Subscriptions Top Up

            else if (MainWindow.Transaction.TopUpType == TopUpTypes.Subscription)
            {
                TopUpSubscription(rfidCard);
            }

            #endregion Subscriptions Top Up
        }

        private void TopUpPoints(RfidCard rfidCard)
        {
            var response = rfidCardRequest.PatchPoints(rfidCard.Uid, Convert.ToInt32(rfidCard.PointsBalance), MainWindow.Transaction.TopUpPoints);
        }

        private void TopUpSubscription(RfidCard rfidCard)
        {
            MainWindow.Transaction.RfidCardSubscription.IdRfidCard = rfidCard.Id;
            if (MainWindow.Transaction.RfidCardSubscription.DateStart == null || MainWindow.Transaction.RfidCardSubscription.DateStart < DateTime.Now)
            {
                MainWindow.Transaction.RfidCardSubscription.DateStart = DateTime.Now;
                MainWindow.Transaction.RfidCardSubscription.DateEnd = DateTime.Now + MainWindow.Transaction.SubscriptionTimespan;
            }
            if (rfidCardSubscriptionRequest.Exists(MainWindow.CardUid, MainWindow.Transaction.RfidCardSubscription.DateStart, MainWindow.Transaction.RfidCardSubscription.DateEnd) == true)
                throw new Exception("Subscription for given dates already exists for this card!");
            var response = rfidCardSubscriptionRequest.Post(MainWindow.Transaction.RfidCardSubscription);
        }
    }
}