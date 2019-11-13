using System;
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
using System.Management;
using System.Data.SqlClient;
using System.Diagnostics;
using DownhillPayClient.UserControls;
using APIClient;
using APIClient.Models;
using APIClient.Requests;
using ArduinoRFIDReader;
using DownhillPayClient.Classes.Transactions;
using DownhillPayClient.APIClient.Requests;
using DownhillPayClient.APIClient.Models;
using System.Net.NetworkInformation;
using DownhillPayClient.Properties;
using System.Net.Sockets;

namespace DownhillPayClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region USER CONTROLS PROPERTIES
        public POSMainMenuView POSMainMenuView { get; }
        public CardReadingUserControl CardReadingUserControl { get; }
        public CheckPointsBalanceUserControl CheckPointsBalanceUserControl { get; }
        public NewCardUserControl NewCardUserControl { get; }
        public PaymentMethod PaymentMethodUserControl { get; }
        public TopUpTypesUserControl TopUpTypesUserControl { get; }
        public NewPersonalizedCardFormUserControl NewPersonalizedCardFormUserControl { get; }
        public PointTopUpUserControl PointTopUpUserControl { get; }
        public SubscriptionTopUpUserControl SubscriptionTopUpUserControl { get; }
        #endregion

        #region PROPERTIES
        public Client Client { get; set; }
        public MFRC522ReaderWriter MFRC522ReaderWriter { get; set; }
        public string CardUid { get; set; }
        public int PaymentValue { get; set; }
        public Transaction Transaction { get; set; }
        #endregion
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                MFRC522ReaderWriter = new MFRC522ReaderWriter(Properties.Settings.Default.RFID_USB_PORT, 9600);
                if (IsApiAvailable() == false)
                {
                    MessageBox.Show("Unable to call API!", "Error!");
                    System.Windows.Application.Current.Shutdown();
                }
                else if (MFRC522ReaderWriter.IsAvailable() == false)
                {
                    MessageBox.Show("RFID card reader is not connected or wrong port has been set!", "Error!");
                    System.Windows.Application.Current.Shutdown();
                }
                else
                {
                    POSMainMenuView = new POSMainMenuView(this);
                    CardReadingUserControl = new CardReadingUserControl(this);
                    CheckPointsBalanceUserControl = new CheckPointsBalanceUserControl(this);
                    NewCardUserControl = new NewCardUserControl(this);
                    PaymentMethodUserControl = new PaymentMethod(this);
                    TopUpTypesUserControl = new TopUpTypesUserControl(this);
                    NewPersonalizedCardFormUserControl = new NewPersonalizedCardFormUserControl(this);
                    PointTopUpUserControl = new PointTopUpUserControl(this);
                    SubscriptionTopUpUserControl = new SubscriptionTopUpUserControl(this);
                    this.contentControl.Content = POSMainMenuView;
                    

                    //tests
                    //ClientRequest clientRequest = new ClientRequest();
                    //var sth = clientRequest.Post(new Client("Jarosław", "Miotła", "631-124-111", new System.DateTime(1999, 07, 12)));
                    //RfidCardRequest cardRequest = new RfidCardRequest();
                    //var cardRequestResponse = cardRequest.Post(new RfidCard("00 00 00 00", "00 00 00 00", "1", "200", DateTime.Now, DateTime.Now));
                    //Debug.WriteLine(cardRequestResponse);
                    //var sth = clientRequest.Get();
                    //Debug.WriteLine(clientRequest.EndpointUri);
                    //Debug.WriteLine(sth[0].FirstName);

                    //MessageBox.Show(Properties.Settings.Default.API_URI);
                    //var tc = new TESTCards();
                    //tc.PayByBLIK(777123);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Error!");
                System.Windows.Application.Current.Shutdown();
            }
        }
        private bool IsApiAvailable()
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("localhost", 3000);
                tcpClient.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                tcpClient.Dispose();
            }
        }
    }
}
