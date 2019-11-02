using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIClient.Models;

namespace DownhillPayClient.Classes.Transactions
{
    /// <summary>
    /// Stores data given in "New card" operation until it's send to database. 
    public class NewCardTransaction : Transaction
    /// </summary>
    {
        public NewCardTransaction() : base()
        {
            this.PaymentValue += 1000;
        }

        public bool IsPersonalized { get; set; }
        public Client Client { get; set; }

    }
}
