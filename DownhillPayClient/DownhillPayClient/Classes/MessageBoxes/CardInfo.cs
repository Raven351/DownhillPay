using DownhillPayClient.APIClient.Models;
using DownhillPayClient.APIClient.Requests;
using DownhillPayClient.MessageBoxLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.Classes.MessageBoxes
{
    public class CardInfo
    {
        /// <summary>
        /// Builder class for MessageBoxLayoutInfo with card's information as messagebox message.
        /// </summary>
        /// <param name="card">Card object of which information are to be displayed</param>
        public CardInfo(RfidCard card)
        {
            this.card = card;
            this.messageBox = new MessageBoxLayoutInfo();
            this.rfidCardSubscriptionRequest = new RfidCardSubscriptionRequest();
            this.subscription = rfidCardSubscriptionRequest.GetClosestUpcoming(card.Id);
        }
        private readonly RfidCard card;
        private readonly MessageBoxLayoutInfo messageBox;
        private readonly RfidCardSubscriptionRequest rfidCardSubscriptionRequest;
        private readonly RfidCardSubscription subscription;
        /// <summary>
        /// Gets messagebox object that contains message parameter with card's info.
        /// </summary>
        /// <returns></returns>
        public MessageBoxLayoutInfo GetMessageBox()
        {
            messageBox.Message = "Card no. " + card.CardNumber + "\n\n Points balance: " + card.PointsBalance + "\n";
            if (subscription.DateEnd < DateTime.Now) messageBox.Message += "\nSubscription:\n No active or upcoming subscriptions";
            else if (rfidCardSubscriptionRequest.Exists(card.Uid, subscription.DateStart, subscription.DateEnd))
                messageBox.Message += "\nSubscription active until\n " + subscription.DateEnd.Year + "-" + subscription.DateEnd.Month + "-" + subscription.DateEnd.Day + "\n"
                    + subscription.DateEnd.Hour + ":" + subscription.DateEnd.Minute;
            else messageBox.Message += "\nSubscription starts on\n " + subscription.DateStart.Year + "-" + subscription.DateStart.Month + "-" + subscription.DateStart.Day + " "
                    + subscription.DateStart.Hour + ":" + subscription.DateStart.Minute +
                    "\n ends on\n " + subscription.DateEnd.Year + "-" + subscription.DateEnd.Month + "-" + subscription.DateEnd.Day + " "
                    + subscription.DateEnd.Hour + ":" + subscription.DateEnd.Minute;
            return messageBox;
        }
    }
}
