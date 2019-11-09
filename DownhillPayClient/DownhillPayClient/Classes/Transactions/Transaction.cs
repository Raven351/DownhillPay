using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.Classes.Transactions
{
    /// <summary>
    /// Basic transaction model. Stores card data until it's send to database. Used in "create card" and "top-up" card operations.
    /// </summary>
    public class Transaction
    {
        public Transaction()
        {
            RfidCard = new APIClient.Models.RfidCard();
            RfidCardSubscription = new APIClient.Models.RfidCardSubscription();
        }

        #region PROPERTIES
        public int PaymentValue { get; set; }
        public TopUpTypes TopUpType { get; set; }
        public APIClient.Models.RfidCard RfidCard { get; set; }
        public int TopUpPoints { get; set; }
        public TimeSpan SubscriptionTimespan { get; set; }
        public APIClient.Models.RfidCardSubscription RfidCardSubscription { get; set; }
        #endregion
    }
}
