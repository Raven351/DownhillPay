using DownhillPayClient.APIClient.Models;
using DownhillPayClient.Properties;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillPayClient.APIClient.Requests
{
    public class RfidCardSubscriptionRequest : Request
    {
        public RfidCardSubscriptionRequest() : base()
        {
            EndpointUri = base.BaseUrl + APIEndpoints.Default.RfidCardSubscription;
        }

        public RfidCardSubscription GetClosestUpcoming(Guid cardId)
        {
            var request = new RestRequest(EndpointUri + "date_end=gte.now()&order=date_end.asc&limit=1&id_rfid_card=eq." + cardId);
            request.AddHeader("Accept", "application/vnd.pgrst.object+json");
            var response = this.Get<RfidCardSubscription>(request);
            return response.Data;
                //date_end=gte.now()&order=date_end.asc&limit=1&id_rfid_card=eq.cec98324-6139-4e2f-99e5-849eba5b867d
        }

        public bool Exists(string cardUid, DateTime startDate, DateTime endDate)
        {
            RfidCardRequest rfidCardRequest = new RfidCardRequest();
            var rfidCardRequestResponse = rfidCardRequest.Get(cardUid);
            if (rfidCardRequestResponse == null) throw new NullReferenceException("Invalid card!");
            var request = new RestRequest(
                EndpointUri + "or=(and(date_start.lte." + startDate + "," + "date_end.gte." + startDate + 
                "),and(date_start.lte." + endDate + "," + "date_end.gte." + endDate + "))&id_rfid_card=eq." + rfidCardRequestResponse.Id);
            var response = this.Get<List<RfidCardSubscription>>(request);
            if (response.Data.Count == 0) return false;
            else return true;

            //or=(and(date_start.lte.2019-01-05,date_end.gte.2019-01-05),and(date_start.lte.2019-03-05,date_end.gte.2019-03-05))&id=eq.9a69917a-7fdb-4092-bb5b-6ae6077216ac
        }
    }
}
