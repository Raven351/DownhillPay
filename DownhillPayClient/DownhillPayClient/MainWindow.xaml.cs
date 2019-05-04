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

namespace DownhillPayClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region USER CONTROLS PROPERTIES
        public POSMainMenuView POSMainMenuView { get; }
        public CheckPointsBalanceUserControl CheckPointsBalanceUserControl { get; }
        public NewCardUserControl NewCardUserControl { get; }
        public PaymentMethod PaymentMethod { get; }
        public TopUpTypesUserControl TopUpTypesUserControl { get; }
        public NewPersonalizedCardFormUserControl NewPersonalizedCardFormUserControl { get; }
        public PointTopUpUserControl PointTopUpUserControl { get; }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            POSMainMenuView = new POSMainMenuView(this);
            CheckPointsBalanceUserControl = new CheckPointsBalanceUserControl(this);
            NewCardUserControl = new NewCardUserControl(this);
            PaymentMethod = new PaymentMethod(this);
            TopUpTypesUserControl = new TopUpTypesUserControl(this);
            NewPersonalizedCardFormUserControl = new NewPersonalizedCardFormUserControl(this);
            PointTopUpUserControl = new PointTopUpUserControl(this);
            this.contentControl.Content = POSMainMenuView;


            //tests
            ClientRequest clientRequest = new ClientRequest();
            //var sth = clientRequest.Post(new Client("Piotr", "Miotła", "631-124-111", "1999-07-12"));
            var sth = clientRequest.Get(6);
            
            Debug.WriteLine(sth.Id);

            //MessageBox.Show(Properties.Settings.Default.API_URI);
            //var tc = new TESTCards();
            //tc.PayByBLIK(777123);
        }
    }
}
